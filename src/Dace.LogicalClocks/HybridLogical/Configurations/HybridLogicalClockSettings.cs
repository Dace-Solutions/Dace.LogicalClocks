namespace Dace.LogicalClocks.HybridLogical.Configurations;
public class HybridLogicalClockSettings
{
    public TimeSpan MaxBackwardJump { get; set; }
    public TimeSpan MaxForwardJump { get; set; }
    public bool ForwardClockJumpCheckEnabled { get; set; } = false;
}
