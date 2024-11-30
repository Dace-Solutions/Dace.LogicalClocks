namespace Dace.LogicalClocks.Internal;
internal class SystemWallClock : IWallClock
{
    public WallClockTimestamp Now()
    {
        return new(DateTime.UtcNow);
    }
}
