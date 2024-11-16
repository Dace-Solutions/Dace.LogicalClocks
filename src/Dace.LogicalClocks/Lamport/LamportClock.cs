namespace Dace.LogicalClock;

using Dace.LogicalClocks;
using Dace.LogicalClocks.Lamport;

public sealed partial class LamportClock :
    ILogicalClock<LamportClockTimestamp>,
    IComparable<LamportClock>,
    IEqualityComparer<LamportClock>,
    IEquatable<LamportClock>
{
    private long _clock = 0;

    private LamportClock() { }

    static internal LamportClock Create()
        => new LamportClock();

    public LamportClockTimestamp Current()
        => new LamportClockTimestamp(_clock);

    public void Witness(
        LamportClockTimestamp receiveClock)
    {
        var currentClock = 0L;

        do
        {
            currentClock = Interlocked.Read(ref _clock);
            if (receiveClock.Clock < currentClock)
            {
                Interlocked.Increment(ref _clock);
                return;
            }
        }
        while (Interlocked.CompareExchange(ref _clock, Math.Max(currentClock, receiveClock.Clock) + 1, currentClock) != currentClock);
    }

    public void Tick()
        => Interlocked.Increment(ref _clock);

    ILogicalClockTick ILogicalClock.Current()
        => Current();

    void ILogicalClock.Witness(
        ILogicalClockTick received)
        => Witness((LamportClockTimestamp)received);

    public static LamportClock Default => LamportClockInstance.LamportClock;

    private class LamportClockInstance
    {
        public static readonly LamportClock LamportClock;

        static LamportClockInstance()
        {
            LamportClock = Create();
        }
    }
}
