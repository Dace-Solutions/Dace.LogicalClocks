using Dace.LogicalClocks.Configurations;
using Dace.LogicalClocks.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dace.LogicalClocks.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogicalClock(
        this IServiceCollection services,
        Action<LogicalClockConfigurator> action)
    {
        var configurator = new LogicalClockConfigurator(services);
        action(configurator);
        return services;
    }
}
