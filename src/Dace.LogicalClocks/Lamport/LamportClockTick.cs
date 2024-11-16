namespace Dace.LogicalClocks.Lamport;

public readonly struct LamportClockTimestamp(long clock)
    : ILogicalClockTick
{
    public long Clock => clock;
}
