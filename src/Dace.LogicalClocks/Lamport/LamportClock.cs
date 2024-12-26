namespace Dace.LogicalClocks.Lamport;

using Dace.LogicalClocks;

/// <summary>
/// Represents a Lamport logical clock, a type of logical clock used to determine the order of events
/// in distributed systems.
/// </summary>
public sealed class LamportClock :
    LogicalClock<LamportClockTimestamp>
{
    private ulong _time = 0L;

    /// <summary>
    /// Gets the default singleton instance of the Lamport clock.
    /// </summary>
    public static LamportClock Default
        => LamportClockInstance.LamportClock;

    private LamportClock() { }

    /// <summary>
    /// Updates the Lamport clock by witnessing a received timestamp from another Lamport clock.
    /// This ensures that the clock accounts for the received timestamp while maintaining
    /// the logical clock ordering constraints.
    /// </summary>
    /// <param name="receiveClock">The received <see cref="LamportClockTimestamp"/>.</param>
    public override LamportClockTimestamp Witness(
        LamportClockTimestamp receiveClock)
    {
        var currentTime = 0UL;

        do
        {
            currentTime = Interlocked.CompareExchange(ref _time, 0UL, 0UL);
            if (receiveClock.Time < currentTime)
            {
                return Now();
            }
        }
        while (Interlocked.CompareExchange(ref _time, Math.Max(currentTime, receiveClock.Time) + 1, currentTime) != currentTime);

        return new(_time);
    }

    /// <summary>
    /// Advances the Lamport clock by one tick.
    /// </summary>
    public override LamportClockTimestamp Now()
    {
        var newTime = Interlocked.Increment(ref _time);
        return new(newTime);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LamportClock"/> class.
    /// </summary>
    /// <returns>A new instance of <see cref="LamportClock"/>.</returns>
    static internal LamportClock Create()
    {
        return new LamportClock();
    }

    /// <summary>
    /// Holds the default singleton instance of the <see cref="LamportClock"/>.
    /// </summary>
    private class LamportClockInstance
    {
        public static readonly LamportClock LamportClock;

        static LamportClockInstance()
        {
            LamportClock = Create();
        }
    }
}
