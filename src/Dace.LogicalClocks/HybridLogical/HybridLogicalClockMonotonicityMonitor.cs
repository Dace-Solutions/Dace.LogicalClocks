namespace Dace.LogicalClocks.HybridLogical;

using Dace.LogicalClocks.HybridLogical.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Timers;

internal class HybridLogicalClockMonotonicityMonitor : IHostedService, IDisposable
{
    private readonly Timer _timer;
    private readonly HybridLogicalClock _hybridLogicalClock;
    private readonly IOptions<HybridLogicalClockSettings> _options;

    public HybridLogicalClockMonotonicityMonitor(
        HybridLogicalClock hybridLogicalClock,
        IOptions<HybridLogicalClockSettings> options)
    {
        _options = options;
        _hybridLogicalClock = hybridLogicalClock;

        _timer = new();
        _timer.Elapsed += (_, _) => Monitor();
        _timer.AutoReset = false;
    }

    private void Monitor()
    {
        try
        {
            if (_options.Value.ForwardClockJumpCheckEnabled)
            {
                _hybridLogicalClock.Now();
            }
            SetTimerInterval();
        }
        finally
        {
            _timer.Start();
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        SetTimerInterval();
        _timer.Start();
        return Task.CompletedTask;
    }

    private void SetTimerInterval()
    {
        _timer.Interval = _options.Value.MaxForwardJump.TotalMilliseconds / 10;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
