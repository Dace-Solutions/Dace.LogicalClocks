namespace Dace.LogicalClocks;

/// <summary>
/// Represents the timestamp of a logical clock. Different logical clocks may contain different properties and manage the clock differently.
/// </summary>
public interface ILogicalClockTimestamp :
    IComparable<ILogicalClockTimestamp>,
    IComparable,
    IEqualityComparer<ILogicalClockTimestamp>,
    IEquatable<ILogicalClockTimestamp>
{
}
