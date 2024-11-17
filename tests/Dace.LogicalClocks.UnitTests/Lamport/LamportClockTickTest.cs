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
        var actualClockValue = stu.Time;

        // Assert
        Assert.Equal(expectedClockValue, actualClockValue);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForEqualClocks()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);

        // Assert
        Assert.True(clock1.Equals(clock1, clock2));
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualClocks()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);
        Assert.Equal(clock1.GetHashCode(clock1), clock2.GetHashCode(clock2));
    }

    [Fact]
    public void CompareTo_ShouldReturnZeroForEqualClocks()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);

        // Assert
        Assert.Equal(0, clock1.CompareTo(clock2));
    }
}
