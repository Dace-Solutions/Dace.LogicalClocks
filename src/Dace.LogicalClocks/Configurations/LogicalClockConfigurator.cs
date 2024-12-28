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
        Action<HybridLogicalClockSettings> configurator)
    {
        _services
            .AddSingleton<HybridLogicalClock>()
            .AddTransient<ILogicalClock>(s => s.GetRequiredService<HybridLogicalClock>())
            .AddHostedService<HybridLogicalClockMonotonicityMonitor>()
            .AddOptions<HybridLogicalClockSettings>()
            .Configure(configurator);

        return this;
    }
}
