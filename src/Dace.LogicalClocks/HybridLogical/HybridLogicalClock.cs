namespace Dace.LogicalClocks.HybridLogical;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed partial class HybridLogicalClock :
    LogicalClock<HybridLogicalClockTimestamp>
{
    private readonly object _lock = new();
    private readonly ILogger<HybridLogicalClock>? _logger;
    private readonly IWallClock _wallClock;
    private readonly HybridLogicalClockSettings _settings;
    private long _lastPhysicalTime;
    private int _logicalTime;
    private int _monotonicityErrorsCount = 0;

    private HybridLogicalClock(
        IWallClock wallClock,
        IOptions<HybridLogicalClockSettings> options,
        ILogger<HybridLogicalClock>? logger)
    {
        _wallClock = wallClock;
        _logger = logger;
        _settings = options.Value;
    }

    /// <summary>
    /// Gets the current timestamp of the Hybrid Logical clock.
    /// </summary>
    /// <returns>The current <see cref="HybridLogicalClockTimestamp"/>.</returns>
    public override HybridLogicalClockTimestamp Current()
        => new HybridLogicalClockTimestamp(_lastPhysicalTime, _logicalTime);

    /// <summary>
    /// Updates the Hybrid Logical clock by witnessing a received timestamp from another Hybrid Logical clock.
    /// This ensures that the clock accounts for the received timestamp while maintaining
    /// the logical clock ordering constraints.
    /// </summary>
    /// <param name="receiveClock">The received <see cref="HybridLogicalClockTimestamp"/>.</param>
    public override HybridLogicalClockTimestamp Witness(
        HybridLogicalClockTimestamp receiveClock)
    {
        if (receiveClock.WallTime < Interlocked.Read(ref _lastPhysicalTime))
            return Current();

        lock (_lock)
        {
            if (receiveClock.WallTime > _lastPhysicalTime)
            {
                _lastPhysicalTime = receiveClock.WallTime;
                _logicalTime = receiveClock.LogicalTime + 1;
            }
            else if (receiveClock.WallTime == _lastPhysicalTime)
            {
                if (receiveClock.LogicalTime > _logicalTime)
                {
                    _logicalTime = Math.Max(receiveClock.LogicalTime, _logicalTime) + 1;
                }
            }
            EnforceWallTimeWithinBoundLocked();
            return new HybridLogicalClockTimestamp(_lastPhysicalTime, _logicalTime);
        }
    }

    /// <summary>
    /// Advances the Hybrid Logical clock by one tick.
    /// </summary>
    public override HybridLogicalClockTimestamp Tick()
    {
        var physicalClock = GetPhysicalClockAndCheck();

        lock (_lock)
        {
            if (_lastPhysicalTime> physicalClock)
            {
                _logicalTime++;
            }
            else
            {
                _lastPhysicalTime = physicalClock;
                _logicalTime = 0;
            }

            EnforceWallTimeWithinBoundLocked();
            return new HybridLogicalClockTimestamp(_lastPhysicalTime, _logicalTime);
        }
    }

    private long GetPhysicalClockAndCheck()
    {
        var oldTime = Interlocked.Read(ref _lastPhysicalTime);
        var newTime = GetWallClockTime();

        while (true)
        {
            var lastPhysTime = Interlocked.Read(ref _lastPhysicalTime);
            if (Interlocked.CompareExchange(ref _lastPhysicalTime, newTime, lastPhysTime) == lastPhysTime)
                break;

            if (lastPhysTime >= newTime)
                break;
        }

        CheckPhysicalClock(oldTime, newTime);
        return newTime;
    }

    private long GetWallClockTime()
    {
        return new DateTimeOffset(_wallClock.Now().Time).ToUnixTimeMilliseconds();
    }

    private void CheckPhysicalClock(long oldTime, long newTime)
    {
        if (oldTime == 0)
            return;

        var interval = oldTime - newTime;
        if (interval > _settings.MaxOffset.Ticks / 10)
        {
            Interlocked.Increment(ref _monotonicityErrorsCount);
            _logger?.LogWarning("Backward time jump detected ({TotalSeconds} seconds)", TimeSpan.FromTicks(-interval).TotalSeconds);
        }

        if (_settings.ForwardClockJumpCheckEnabled)
        {
            var toleratedForwardClockJump = ToleratedForwardClockJump();
            if (toleratedForwardClockJump.Ticks <= -interval)
            {
                _logger?.LogCritical(
                    "Detected forward time jump of {TotalSeconds} seconds is not allowed with tolerance of {ToleratedForwardClockJump} seconds",
                    TimeSpan.FromMilliseconds(-interval).TotalSeconds,
                    toleratedForwardClockJump.TotalSeconds);
            }
        }
    }

    private void EnforceWallTimeWithinBoundLocked()
    {
        if (_settings.WallTimeUpperBound != 0 && _lastPhysicalTime > _settings.WallTimeUpperBound)
        {
            _logger?.LogCritical(
                "Wall time {PhysicalTime} is not allowed to be greater than upper bound of {WallTimeUpperBound}",
                _lastPhysicalTime,
                _settings.WallTimeUpperBound);
        }
    }

    private TimeSpan ToleratedForwardClockJump()
    {
        return _settings.MaxOffset / 2;
    }
}
