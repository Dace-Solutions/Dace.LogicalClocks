namespace Dace.LogicalClocks.HybridLogical;

using Dace.LogicalClocks.Extensions;
using Dace.LogicalClocks.HybridLogical.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Timers;

internal class HybridLogicalClockMonotonicityMonitor : IHostedService, IDisposable
{
    private readonly Timer _timer;
    private readonly HybridLogicalClock _hybridLogicalClock;
    private readonly IOptions<HybridLogicalClockMonotonicityMonitorSettings> _options;
    private readonly ILogger<HybridLogicalClockMonotonicityMonitor>? _logger;
    private bool _disposed = false;

    public HybridLogicalClockMonotonicityMonitor(
        HybridLogicalClock hybridLogicalClock,
        IOptions<HybridLogicalClockMonotonicityMonitorSettings> options,
        ILogger<HybridLogicalClockMonotonicityMonitor>? logger)
    {
        _hybridLogicalClock = hybridLogicalClock;
        _options = options;
        _logger = logger;
        _timer = new();
        _timer.Elapsed += (_, _) => Monitor();
        _timer.AutoReset = false;
    }

    private void Monitor()
    {
        try
        {
            _hybridLogicalClock.Now();
            SetTimerInterval();
        }
        catch(Exception ex)
        {
            _logger?.ExceptionInMonotonicityMonitor(ex);
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
        _timer.Interval = _options.Value.Interval.TotalMilliseconds;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_disposed) 
            return;

        _timer.Dispose();
        _disposed = true;
    }
}
