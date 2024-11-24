namespace Dace.LogicalClocks.UnitTests.Lamport;

using Dace.LogicalClocks.Lamport;
using Moq;

public class LamportClockTest
{
    [Fact]
    public void Default_ShouldInitializeClock()
    {
        var clock = LamportClock.Default;
        var currentClock = clock.Current();
        Assert.Equal(0, currentClock.Time);
    }

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
    public async Task CurrentAsync_LamportClockTimestamp_ShouldReturnClockTick()
    {
        ILogicalClock<LamportClockTimestamp> clock = LamportClock.Create();
        var currentClock = await clock.CurrentAsync();
        Assert.Equal(0, currentClock.Time);
    }

    [Fact]
    public async Task CurrentAsync_ShouldReturnClockTick()
    {
        ILogicalClock clock = LamportClock.Create();
        var currentClock = (LamportClockTimestamp)await clock.CurrentAsync();
        Assert.Equal(0, currentClock.Time);
    }

    [Fact]
    public void Witness_ShouldUpdateClock()
    {
        var clock = LamportClock.Create();
        var receivedClock = new LamportClockTimestamp(5);
        var result = clock.Witness(receivedClock);
        var currentClock = clock.Current();
        Assert.Equal(6, currentClock.Time);
        Assert.Equal(result.Time, currentClock.Time);
    }

    [Fact]
    public void Witness_ShouldNotUpdateClock_WhenTheClockIsLessThanCurrent()
    {
        var clock = LamportClock.Create();
        clock.Tick();//Current:1
        clock.Tick();//Current:2
        clock.Tick();//Current:3
        clock.Tick();//Current:4
        clock.Tick();//Current:5

        var receivedClock = new LamportClockTimestamp(3);
        var result = clock.Witness(receivedClock);
        var currentClock = clock.Current();
        Assert.Equal(6, currentClock.Time);

        Assert.Equal(result.Time, currentClock.Time);
    }

    [Fact]
    public async Task WitnessAsync_LamportClockTimestamp_ShouldUpdateClock()
    {
        ILogicalClock<LamportClockTimestamp> clock = LamportClock.Create();
        var receivedClock = new LamportClockTimestamp(5);
        var result = await clock.WitnessAsync(receivedClock);
        var currentClock = await clock.CurrentAsync();
        Assert.Equal(6, currentClock.Time);
        Assert.Equal(result.Time, currentClock.Time);
    }

    [Fact]
    public async Task WitnessAsync_ShouldUpdateClock()
    {
        ILogicalClock clock = LamportClock.Create();
        var receivedClock = new LamportClockTimestamp(5);
        var receivedClockMock = new Mock<ILogicalClockTimestamp>();

        var result = (LamportClockTimestamp)await clock.WitnessAsync(receivedClock);
        var currentClock = (LamportClockTimestamp)await clock.CurrentAsync();

        Assert.Equal(6, currentClock.Time);
        Assert.Equal(result.Time, currentClock.Time);
    }

    [Fact]
    public async Task WitnessAsync_ThorwExceptionOnDifferentTimestampType()
    {
        ILogicalClock clock = LamportClock.Create();
        var receivedClockMock = new Mock<ILogicalClockTimestamp>();

        await Assert.ThrowsAsync<ArgumentException>(async () => await clock.WitnessAsync(receivedClockMock.Object));
    }

    [Fact]
    public void Tick_ShouldIncrementClock()
    {
        var clock = LamportClock.Create();
        var result = clock.Tick();
        var currentClock = clock.Current();
        Assert.Equal(1, currentClock.Time);
        Assert.Equal(result.Time, currentClock.Time);
    }

    [Fact]
    public async Task TickAsync_LamportClockTimestamp_ShouldIncrementClock()
    {
        ILogicalClock<LamportClockTimestamp> clock = LamportClock.Create();
        var result = await clock.TickAsync();
        var currentClock = await clock.CurrentAsync();
        Assert.Equal(1, currentClock.Time);
        Assert.Equal(result.Time, currentClock.Time);
    }

    [Fact]
    public async Task TickAsync_ShouldIncrementClock()
    {
        ILogicalClock clock = LamportClock.Create();
        var result = (LamportClockTimestamp)await clock.TickAsync();
        var currentClock = (LamportClockTimestamp)await clock.CurrentAsync();
        Assert.Equal(1, currentClock.Time);
        Assert.Equal(result.Time, currentClock.Time);
    }
}
