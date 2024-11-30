namespace Dace.LogicalClocks.HybridLogical;
public class HybridLogicalClockSettings
{
    public TimeSpan MaxOffset { get; set; }
    public TimeSpan ToleratedOffset { get; set; }
    public bool ForwardClockJumpCheckEnabled { get; set; } = false;
    public bool IsMonitoringForwardClockJumps { get; set; } = false;
    public long WallTimeUpperBound { get; set; } = 0;
}
