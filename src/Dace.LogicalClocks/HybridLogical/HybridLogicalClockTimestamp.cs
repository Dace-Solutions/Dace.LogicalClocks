namespace Dace.LogicalClocks.HybridLogical;

public readonly struct HybridLogicalClockTimestamp(WallClockTimestamp wallTime, int logicalTime)
    : ILogicalClockTimestamp,
    IComparable<HybridLogicalClockTimestamp>,
    IComparable,
    IEquatable<HybridLogicalClockTimestamp>
{
    /// <summary>
    /// Represents the physical time in Unix Epoch.
    /// </summary>
    public WallClockTimestamp WallTime => wallTime;
    /// <summary>
    /// Represents the logical time.
    /// </summary>
    public int LogicalTime => logicalTime;

    /// <inheritdoc />
    public bool Equals(
        HybridLogicalClockTimestamp other)
    {
        return WallTime == other.WallTime && LogicalTime == other.LogicalTime;
    }

    /// <inheritdoc />
    int IComparable<ILogicalClockTimestamp>.CompareTo(
        ILogicalClockTimestamp? other)
    {
        if (other is null)
            return 1; // Null is considered less than any valid clock
        if (other is HybridLogicalClockTimestamp hybridLogicalClockTimestamp)
            return CompareTo(hybridLogicalClockTimestamp);
        throw new ArgumentException($"The provided clock is not a {nameof(HybridLogicalClockTimestamp)}.", nameof(other));
    }

    /// <inheritdoc />
    int IComparable.CompareTo(object? other)
    {
        return other is HybridLogicalClockTimestamp hybridLogicalClockTimestamp
            ? CompareTo(hybridLogicalClockTimestamp)
            : throw new ArgumentException($"The provided clock is not a {nameof(HybridLogicalClockTimestamp)}.", nameof(other));
    }

    /// <inheritdoc />
    bool IEquatable<ILogicalClockTimestamp>.Equals(
        ILogicalClockTimestamp? other)
    {
        return other is HybridLogicalClockTimestamp hybridLogicalClockTimestamp
            && Equals(hybridLogicalClockTimestamp);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is HybridLogicalClockTimestamp hybridLogicalClockTimestamp
            && Equals(hybridLogicalClockTimestamp);
    }

    /// <summary>
    /// Compares the current Lamport clock timestamp with another Lamport clock timestamp based on their time.
    /// </summary>
    /// <param name="other">The other <see cref="HybridLogicalClockTimestamp"/> to compare to.</param>
    /// <returns>
    /// A value less than 0 if this clock is less than <paramref name="other"/>,
    /// 0 if they are equal, or a value greater than 0 if this clock is greater.
    /// </returns>
    public int CompareTo(HybridLogicalClockTimestamp other)
    {
        return WallTime == other.WallTime
            ? LogicalTime.CompareTo(other.LogicalTime)
            : WallTime.CompareTo(other.WallTime);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(WallTime, LogicalTime);
    }

    /// <summary>
    /// Determines whether two <see cref="HybridLogicalClockTimestamp"/> instances are not equal.
    /// </summary>
    public static bool operator !=(
        HybridLogicalClockTimestamp? left,
        HybridLogicalClockTimestamp? right)
        => !(left == right);

    /// <summary>
    /// Determines whether two <see cref="HybridLogicalClockTimestamp"/> instances are equal.
    /// </summary>
    public static bool operator ==(
        HybridLogicalClockTimestamp? left,
        HybridLogicalClockTimestamp? right)
    {
        if (left is null || right is null)
            return left is null && right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether one <see cref="HybridLogicalClockTimestamp"/> instance is greater than another.
    /// </summary>
    public static bool operator >(HybridLogicalClockTimestamp left, HybridLogicalClockTimestamp right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether one <see cref="HybridLogicalClockTimestamp"/> instance is less than another.
    /// </summary>
    public static bool operator <(HybridLogicalClockTimestamp left, HybridLogicalClockTimestamp right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether one <see cref="HybridLogicalClockTimestamp"/> instance is greater than or equal to another.
    /// </summary>
    public static bool operator >=(HybridLogicalClockTimestamp left, HybridLogicalClockTimestamp right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether one <see cref="HybridLogicalClockTimestamp"/> instance is less than or equal to another.
    /// </summary>
    public static bool operator <=(HybridLogicalClockTimestamp left, HybridLogicalClockTimestamp right)
        => left.CompareTo(right) <= 0;
}
