namespace Dace.LogicalClocks.Extensions;
using Microsoft.Extensions.Logging;
using System;

internal static partial class Log
{
    [LoggerMessage(
        EventId = 0, 
        Level = LogLevel.Critical, 
        Message = "Wall time {LastPhysicalTime} ns exceeds maximum allowed value of {WallTimeUpperBound} ns.")]
    public static partial void WallTimeOutOfRangeException(this ILogger logger, double lastPhysicalTime, double wallTimeUpperBound);

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Critical,
        Message = "Critical exception occured during monitoring HLC runs monotonically")]
    public static partial void ExceptionInMonotonicityMonitor(this ILogger logger, Exception ex);
}
