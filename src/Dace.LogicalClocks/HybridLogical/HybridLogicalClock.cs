namespace Dace.LogicalClocks.HybridLogical;

using Dace.LogicalClocks.HybridLogical.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed partial class HybridLogicalClock :
    LogicalClock<HybridLogicalClockTimestamp>
{
    private readonly object _lock = new();
    private readonly ILogger<HybridLogicalClock>? _logger;
    private readonly IWallClock _wallClock;
    private readonly HybridLogicalClockSettings _settings;
    private WallClockTimestamp _lastPhysicalTime = WallClockTimestamp.Zero;
    private int _logicalTime;
    private int _monotonicityErrorsCount = 0;

    internal HybridLogicalClock(
        IWallClock wallClock,
        IOptions<HybridLogicalClockSettings> options,
        ILogger<HybridLogicalClock>? logger)
    {
        _wallClock = wallClock;
        _logger = logger;
        _settings = options.Value;
    }

    /// <summary>
    /// Updates the Hybrid Logical clock by witnessing a received timestamp from another Hybrid Logical clock.
    /// This ensures that the clock accounts for the received timestamp while maintaining
    /// the logical clock ordering constraints.
    /// </summary>
    /// <param name="receiveClock">The received <see cref="HybridLogicalClockTimestamp"/>.</param>
    public override HybridLogicalClockTimestamp Witness(
        HybridLogicalClockTimestamp receiveClock)
    {
        if (receiveClock.WallTime < GetLastPhysicalClock())
            return Now();

        lock (_lock)
        {
            if (receiveClock.WallTime > _lastPhysicalTime)
            {
                _lastPhysicalTime = receiveClock.WallTime;
                _logicalTime = receiveClock.LogicalTime + 1;
            }
            else if (receiveClock.WallTime == _lastPhysicalTime)
            {
                _logicalTime = Math.Max(receiveClock.LogicalTime, _logicalTime) + 1;
            }
            return new HybridLogicalClockTimestamp(_lastPhysicalTime, _logicalTime);
        }
    }

    /// <summary>
    /// Advances the Hybrid Logical clock by one tick.
    /// </summary>
    public override HybridLogicalClockTimestamp Now()
    {
        var physicalClock = GetPhysicalClockAndCheck();

        lock (_lock)
        {
            if (_lastPhysicalTime >= physicalClock)
            {
                _logicalTime++;
            }
            else
            {
                _lastPhysicalTime = physicalClock;
                _logicalTime = 0;
            }

            return new HybridLogicalClockTimestamp(_lastPhysicalTime, _logicalTime);
        }
    }

    private WallClockTimestamp GetPhysicalClockAndCheck()
    {
        var oldTime = GetLastPhysicalClock();
        var newTime = GetWallClockTime();
        var lastPhysicalTime = oldTime;

        do
        {
            if (Interlocked.CompareExchange(ref _lastPhysicalTime, newTime, lastPhysicalTime) == lastPhysicalTime)
                break;

            lastPhysicalTime = GetLastPhysicalClock();
            if (lastPhysicalTime >= newTime)
                break;
        } while (true);


        if (oldTime != WallClockTimestamp.Zero)
        {
            var interval = oldTime.UnixEpoch - newTime.UnixEpoch;
            if (interval > _settings.MaxBackwardJump.TotalNanoseconds)
            {
                Interlocked.Increment(ref _monotonicityErrorsCount);
                _logger?.LogWarning(
                    "Detected backward time jump of {NanoSeconds} nano seconds is not allowed with tolerance of {newTime} nano seconds",
                    interval,
                    _settings.MaxBackwardJump);
            }
            else if (_settings.ForwardClockJumpCheckEnabled && _settings.MaxForwardJump.TotalNanoseconds <= -interval)
            {
                _logger?.LogCritical(
                    "Detected forward time jump of {NanoSeconds} nano seconds is not allowed with tolerance of {MaxForwardJump} nano seconds",
                    -interval,
                    _settings.MaxForwardJump.TotalNanoseconds);
            }
        }

        return newTime;
    }

    private WallClockTimestamp GetLastPhysicalClock()
    {
        return Interlocked.CompareExchange(ref _lastPhysicalTime, WallClockTimestamp.Zero, WallClockTimestamp.Zero);
    }

    private WallClockTimestamp GetWallClockTime()
    {
        return _wallClock.Now();
    }
}
