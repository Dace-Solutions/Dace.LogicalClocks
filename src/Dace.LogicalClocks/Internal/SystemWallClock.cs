namespace Dace.LogicalClocks.Internal;

using Dace.LogicalClocks.Configurations;
using Dace.LogicalClocks.Exceptions;
using Dace.LogicalClocks.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

internal class SystemWallClock(
    IOptions<WallClockSettings>? wallClockSettings,
    ILogger<SystemWallClock>? logger) : IWallClock
{
    private readonly WallClockSettings _settings = wallClockSettings?.Value ?? new WallClockSettings();

    public WallClockTimestamp Now()
    {
        var lastPhysicalTime = new WallClockTimestamp(DateTime.UtcNow);

        if (_settings.WallTimeUpperBound != WallClockTimestamp.Zero
            && lastPhysicalTime > _settings.WallTimeUpperBound)
        {
            var exception = new WallTimeOutOfRangeException(lastPhysicalTime, _settings.WallTimeUpperBound);
            logger?.WallTimeOutOfRangeException(lastPhysicalTime.UnixEpoch, _settings.WallTimeUpperBound.UnixEpoch);
            throw exception;
        }

        return lastPhysicalTime;
    }
}
