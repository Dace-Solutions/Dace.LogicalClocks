namespace Dace.LogicalClocks.HybridLogical;
public class HybridLogicalClockSettings
{
    public TimeSpan MaxBackwardJump { get; set; }
    public TimeSpan MaxForwardJump { get; set; }
    public bool ForwardClockJumpCheckEnabled { get; set; } = false;
    public WallClockTimestamp WallTimeUpperBound { get; set; } = WallClockTimestamp.Zero;
}
