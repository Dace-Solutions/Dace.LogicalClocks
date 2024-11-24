namespace Dace.LogicalClocks.UnitTests.Configurations;

using Dace.LogicalClocks.Configurations;
using Dace.LogicalClocks.Lamport;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class LogicalClockConfiguratorTest
{
    [Fact]
    public void UseLamportClock_ShouldRegisterLamportClock()
    {
        //Arrange
        var serviceCollection = new Mock<IServiceCollection>();
        var stub = new LogicalClockConfigurator(serviceCollection.Object);

        //Act
        stub.UseLamportClock();

        // Assert
        serviceCollection
            .Verify(t => t.Add(It.Is<ServiceDescriptor>(d => d.ServiceType == typeof(LamportClock) && d.Lifetime == ServiceLifetime.Singleton)));
        serviceCollection
            .Verify(t => t.Add(It.Is<ServiceDescriptor>(d => d.ServiceType == typeof(ILogicalClock) && d.Lifetime == ServiceLifetime.Transient)));
    }
}
