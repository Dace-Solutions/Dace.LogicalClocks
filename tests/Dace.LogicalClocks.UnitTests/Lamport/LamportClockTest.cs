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
        Assert.Equal(0, currentClock.Time);
    }

    [Fact]
    public void Witness_ShouldUpdateClock()
    {
        var clock = LamportClock.Create();
        var receivedClock = new LamportClockTimestamp(5);
        clock.Witness(receivedClock);
        var currentClock = clock.Current();
        Assert.Equal(6, currentClock.Time);
    }

    [Fact]
    public void Tick_ShouldIncrementClock()
    {
        var clock = LamportClock.Create();
        clock.Tick();
        var currentClock = clock.Current();
        Assert.Equal(1, currentClock.Time);
    }
}
