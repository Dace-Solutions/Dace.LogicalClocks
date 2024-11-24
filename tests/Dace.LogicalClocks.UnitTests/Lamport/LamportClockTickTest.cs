namespace Dace.LogicalClocks.UnitTests.Lamport;

using Dace.LogicalClocks.Lamport;
using Moq;

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
    public void ObjectEquals_ShouldOnlyReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);

        // Assert
        Assert.True(clock1.Equals((object)clock2));
        Assert.False(clock1.Equals(new object()));
    }

    [Fact]
    public void Equals_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);

        // Assert
        Assert.True(clock1.Equals(clock2));
    }

    [Fact]
    public void EqualsOperator_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);
        LamportClockTimestamp? clock3 = null;
        LamportClockTimestamp? clock4 = null;

        // Assert
        Assert.True(clock1 == clock2);
        Assert.True(clock1 == clock2);
        Assert.False(clock3 == clock1);
        Assert.False(clock1 == clock3);
        Assert.True(clock3 == clock4);
    }

    [Fact]
    public void NotEqualsOperator_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time + 1);

        // Assert
        Assert.True(clock1 != clock2);
    }

    [Fact]
    public void GreaterThanOperator_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time + 1);
        var clock2 = new LamportClockTimestamp(time);

        // Assert
        Assert.True(clock1 > clock2);
    }

    [Fact]
    public void GreaterThanOrEqualsOperator_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time + 1);
        var clock2 = new LamportClockTimestamp(time);

        // Assert
        Assert.True(clock1 >= clock2);
    }

    [Fact]
    public void LessThanOperator_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time + 1);

        // Assert
        Assert.True(clock1 < clock2);
    }

    [Fact]
    public void LessThanOrEqualsOperator_ShouldReturnTrueForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time + 1);

        // Assert
        Assert.True(clock1 <= clock2);
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);
        Assert.Equal(clock1.GetHashCode(), clock2.GetHashCode());
    }

    [Fact]
    public void CompareTo_ShouldReturnZeroForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        var clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);
        var clock3 = new LamportClockTimestamp(time - 1);
        var clock4 = new LamportClockTimestamp(time + 1);

        // Assert
        Assert.Equal(0, clock1.CompareTo(clock2));
        Assert.True(clock1.CompareTo(clock3) > 0);
        Assert.True(clock1.CompareTo(clock4) < 0);
    }

    [Fact]
    public void CompareTo_LogicalClockTimestamp_ShouldReturnZeroForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        IComparable<ILogicalClockTimestamp> clock1 = new LamportClockTimestamp(time);
        ILogicalClockTimestamp clock2 = new LamportClockTimestamp(time);
        ILogicalClockTimestamp clock3 = new LamportClockTimestamp(time - 1);
        ILogicalClockTimestamp clock4 = new LamportClockTimestamp(time + 1);
        var mock = new Mock<ILogicalClockTimestamp>();

        // Assert
        Assert.Equal(1, clock1.CompareTo(null));
        Assert.Equal(0, clock1.CompareTo(clock2));
        Assert.True(clock1.CompareTo(clock3) > 0);
        Assert.True(clock1.CompareTo(clock4) < 0);
        Assert.Throws<ArgumentException>(() => clock1.CompareTo(mock.Object));
    }
    [Fact]
    public void Equals_LogicalClockTimestamp_ShouldReturnZeroForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        IEquatable<ILogicalClockTimestamp> clock1 = new LamportClockTimestamp(time);
        ILogicalClockTimestamp clock2 = new LamportClockTimestamp(time);
        ILogicalClockTimestamp clock3 = new LamportClockTimestamp(time - 1);
        ILogicalClockTimestamp clock4 = new LamportClockTimestamp(time + 1);
        var mock = new Mock<ILogicalClockTimestamp>();

        // Assert
        Assert.True(clock1.Equals(clock2));
        Assert.False(clock1.Equals(clock3));
        Assert.False(clock1.Equals(clock4));
        Assert.False(clock1.Equals(mock.Object));
    }
    [Fact]
    public void CompareTo_IComparable_ShouldReturnZeroForEqualClockTimestamps()
    {
        // Arrange
        var time = DateTime.Now.Ticks;
        IComparable clock1 = new LamportClockTimestamp(time);
        var clock2 = new LamportClockTimestamp(time);
        var clock3 = new LamportClockTimestamp(time - 1);
        var clock4 = new LamportClockTimestamp(time + 1);

        // Assert
        Assert.Equal(0, clock1.CompareTo(clock2));
        Assert.True(clock1.CompareTo(clock3) > 0);
        Assert.True(clock1.CompareTo(clock4) < 0);
        Assert.Throws<ArgumentException>(() => clock1.CompareTo(new object()));
    }
}
