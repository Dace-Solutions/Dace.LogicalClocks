using Dace.LogicalClock;

namespace Dace.LogicalClocks.Configurations;

using Dace.LogicalClocks;
using Microsoft.Extensions.DependencyInjection;

public class LogicalClockConfigurator
{
    private readonly IServiceCollection _services;

    protected internal LogicalClockConfigurator(
        IServiceCollection services)
    {
        _services = services;
    }

    public void UseLamportClock()
    {
        _services
            .AddSingleton(s => LamportClock.Default)
            .AddTransient<ILogicalClock>(s => s.GetRequiredService<LamportClock>());
    }
}
