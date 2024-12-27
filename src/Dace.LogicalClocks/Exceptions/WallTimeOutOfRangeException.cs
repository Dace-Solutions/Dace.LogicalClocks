namespace Dace.LogicalClocks.Exceptions;
public class WallTimeOutOfRangeException : LogicalClockException
{
    public WallClockTimestamp CapturedWallClock { get; }

    public WallClockTimestamp WallTimeUpperBound { get; }

    public WallTimeOutOfRangeException(
        WallClockTimestamp capturedWallClock,
        WallClockTimestamp wallTimeUpperBound)
        : base($"Wall time {capturedWallClock.UnixEpoch} ns exceeds maximum allowed value of {wallTimeUpperBound.UnixEpoch} ns.")
    {
        CapturedWallClock = capturedWallClock;
        WallTimeUpperBound = wallTimeUpperBound;
    }
}
