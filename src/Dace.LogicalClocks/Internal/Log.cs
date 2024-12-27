namespace Dace.LogicalClocks.Extensions;
using Microsoft.Extensions.Logging;

internal static partial class Log
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Critical, Message = "Wall time {LastPhysicalTime} ns exceeds maximum allowed value of {WallTimeUpperBound} ns.")]
    public static partial void WallTimeOutOfRangeException(this ILogger logger, double lastPhysicalTime, double wallTimeUpperBound);
}
