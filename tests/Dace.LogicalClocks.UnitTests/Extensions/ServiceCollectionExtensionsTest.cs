namespace Dace.LogicalClocks.UnitTests.Extensions;

using Dace.LogicalClocks.Configurations;
using Dace.LogicalClocks.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void AddLogicalClock_ShouldCallAction()
    {
        //Arrange
        var serviceCollection = new Mock<IServiceCollection>();
        var action = new Mock<Action<LogicalClockConfigurator>>();

        //Act
        ServiceCollectionExtensions
            .AddLogicalClock(serviceCollection.Object, action.Object);

        //Assert
        action
            .Verify(t => t.Invoke(It.IsAny<LogicalClockConfigurator>()));
    }
}
