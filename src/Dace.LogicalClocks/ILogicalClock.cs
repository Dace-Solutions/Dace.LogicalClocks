namespace Dace.LogicalClocks;

public interface ILogicalClock :
    IComparable<ILogicalClock>,
    IComparable,
    IEqualityComparer<ILogicalClock>,
    IEquatable<ILogicalClock>
{
    void Tick();

    void Witness(ILogicalClockTick received);

    ILogicalClockTick Current();
}
