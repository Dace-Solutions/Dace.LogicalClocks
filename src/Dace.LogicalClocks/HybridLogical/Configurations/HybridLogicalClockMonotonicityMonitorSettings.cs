namespace Dace.LogicalClocks.HybridLogical.Configurations;

internal class HybridLogicalClockMonotonicityMonitorSettings
{
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);
}
