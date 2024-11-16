namespace Dace.LogicalClocks.UnitTests.Lamport;

using Dace.LogicalClocks.Lamport;

public class LamportClockTickTest
{
    [Fact]
    public void ClockProperty_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedClockValue = 12345L;
        var stu = new LamportClockTimestamp(expectedClockValue);

        // Act
        var actualClockValue = stu.Clock;

        // Assert
        Assert.Equal(expectedClockValue, actualClockValue);
    }
}
