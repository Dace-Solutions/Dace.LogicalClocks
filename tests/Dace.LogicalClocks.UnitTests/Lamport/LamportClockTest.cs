using Dace.LogicalClock;

namespace Dace.LogicalClocks.UnitTests.Lamport;

using Dace.LogicalClocks.Lamport;

public class LamportClockTest
{
    [Fact]
    public void Create_ShouldInitializeClock()
    {
        var clock = LamportClock.Create();
        Assert.NotNull(clock);
    }

    [Fact]
    public void Current_ShouldReturnClockTick()
    {
        var clock = LamportClock.Create();
        var currentClock = clock.Current();
        Assert.Equal(0, currentClock.Clock);
    }

    [Fact]
    public void Witness_ShouldUpdateClock()
    {
        var clock = LamportClock.Create();
        var receivedClock = new LamportClockTimestamp(5);
        clock.Witness(receivedClock);
        var currentClock = clock.Current();
        Assert.Equal(6, currentClock.Clock);
    }

    [Fact]
    public void Tick_ShouldIncrementClock()
    {
        var clock = LamportClock.Create();
        clock.Tick();
        var currentClock = clock.Current();
        Assert.Equal(1, currentClock.Clock);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForEqualClocks()
    {
        var clock1 = LamportClock.Create();
        var clock2 = LamportClock.Create();
        Assert.True(clock1.Equals(clock1, clock2));
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualClocks()
    {
        var clock1 = LamportClock.Create();
        var clock2 = LamportClock.Create();
        Assert.Equal(clock1.GetHashCode(clock1), clock2.GetHashCode(clock2));
    }

    [Fact]
    public void CompareTo_ShouldReturnZeroForEqualClocks()
    {
        var clock1 = LamportClock.Create();
        var clock2 = LamportClock.Create();
        Assert.Equal(0, clock1.CompareTo(clock2));
    }
}
