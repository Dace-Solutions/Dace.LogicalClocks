namespace Dace.LogicalClocks.Configurations;

using Dace.LogicalClocks;
using Dace.LogicalClocks.HybridLogical;
using Dace.LogicalClocks.HybridLogical.Configurations;
using Dace.LogicalClocks.Internal;
using Dace.LogicalClocks.Lamport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

public class LogicalClockConfigurator
{
    private readonly IServiceCollection _services;
    private OptionsBuilder<WallClockSettings> _options;

    protected internal LogicalClockConfigurator(
        IServiceCollection services)
    {
        _services = services;

        _services
            .TryAddSingleton<IWallClock, SystemWallClock>();

        _options = _services
             .AddOptions<WallClockSettings>();
    }

    public LogicalClockConfigurator ConfigureWallClock(
        Action<WallClockSettings> configurator)
    {
        _options
            .Configure(configurator);

        return this;
    }

    public LogicalClockConfigurator UseLamportClock()
    {
        _services
            .AddSingleton(s => LamportClock.Default)
            .AddTransient<ILogicalClock>(s => s.GetRequiredService<LamportClock>());

        return this;
    }

    public LogicalClockConfigurator UseHybridLogicalClock(
        Action<HybridLogicalClockConfigurator>? action = null)
    {
        _services
            .AddSingleton<HybridLogicalClock>()
            .AddTransient<ILogicalClock>(s => s.GetRequiredService<HybridLogicalClock>());

        var configurator = new HybridLogicalClockConfigurator();
        if (action is not null)
            action(configurator);

        configurator.Build(_services);

        return this;
    }
}

public class HybridLogicalClockConfigurator
{
    private TimeSpan? _maxBackwardJump;
    private TimeSpan? _maxForwardJump;
    private bool? _checkForwardClockJump;
    private TimeSpan? _forwardClockJumpCheckInterval;

    public HybridLogicalClockConfigurator SetMaxBackwardJump(TimeSpan value)
    {
        _maxBackwardJump = value;
        return this;
    }

    public HybridLogicalClockConfigurator SetMaxForwardJump(TimeSpan value)
    {
        _maxForwardJump = value;
        return this;
    }

    public HybridLogicalClockConfigurator CheckForwardClockJump(bool value)
    {
        _checkForwardClockJump = value;
        return this;
    }

    public HybridLogicalClockConfigurator ForwardClockJumpCheckInterval(TimeSpan value)
    {
        _forwardClockJumpCheckInterval = value;
        return this;
    }

    internal void Build(IServiceCollection services)
    {
        var opt = services
            .AddOptions<HybridLogicalClockSettings>()
            .Configure(Configure);

        if (_checkForwardClockJump ?? false)
        {
            services
                .AddHostedService<HybridLogicalClockMonotonicityMonitor>()
                .AddOptions<HybridLogicalClockMonotonicityMonitorSettings>()
                .Configure(s =>
                {
                    s.Interval = _forwardClockJumpCheckInterval ?? (_maxForwardJump is not null ? _maxForwardJump.Value / 10 : TimeSpan.FromSeconds(1));
                });
        }
    }

    private void Configure(HybridLogicalClockSettings settings)
    {
        if (_maxBackwardJump is not null)
        {
            settings.MaxBackwardJump = _maxBackwardJump.Value;
        }

        if (_maxForwardJump is not null)
        {
            settings.MaxForwardJump = _maxForwardJump.Value;
        }
    }
}
