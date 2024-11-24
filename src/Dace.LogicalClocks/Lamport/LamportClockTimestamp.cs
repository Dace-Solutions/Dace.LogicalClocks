namespace Dace.LogicalClocks.Lamport;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents a timestamp for a Lamport logical clock, encapsulating the logical time.
/// </summary>
/// <param name="time">The logical time represented by the timestamp.</param>
public readonly struct LamportClockTimestamp(long time)
    : ILogicalClockTimestamp,
    IComparable<LamportClockTimestamp>,
    IComparable,
    IEquatable<LamportClockTimestamp>

{
    /// <summary>
    /// Gets the logical time represented by this timestamp.
    /// </summary>
    public long Time => time;

    /// <inheritdoc />
    public bool Equals(
        LamportClockTimestamp other)
    {
        return time == other.Time;
    }

    /// <inheritdoc />
    int IComparable<ILogicalClockTimestamp>.CompareTo(
        ILogicalClockTimestamp? other)
    {
        if (other is null)
            return 1; // Null is considered less than any valid clock
        if (other is LamportClockTimestamp lamportClockTimestamp)
            return CompareTo(lamportClockTimestamp);
        throw new ArgumentException($"The provided clock is not a {nameof(LamportClockTimestamp)}.", nameof(other));
    }

    /// <inheritdoc />
    int IComparable.CompareTo(object? other)
    {
        return other is LamportClockTimestamp lamportClockTimestamp
            ? CompareTo(lamportClockTimestamp)
            : throw new ArgumentException($"The provided clock is not a {nameof(LamportClockTimestamp)}.", nameof(other));
    }

    /// <inheritdoc />
    bool IEquatable<ILogicalClockTimestamp>.Equals(
        ILogicalClockTimestamp? other)
    {
        return other is LamportClockTimestamp lamportClockTimestamp
            && Equals(lamportClockTimestamp);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is LamportClockTimestamp lamportClockTimestamp
            && Equals(lamportClockTimestamp);
    }

    /// <summary>
    /// Compares the current Lamport clock timestamp with another Lamport clock timestamp based on their time.
    /// </summary>
    /// <param name="other">The other <see cref="LamportClockTimestamp"/> to compare to.</param>
    /// <returns>
    /// A value less than 0 if this clock is less than <paramref name="other"/>,
    /// 0 if they are equal, or a value greater than 0 if this clock is greater.
    /// </returns>
    public int CompareTo(LamportClockTimestamp other)
    {
        return time.CompareTo(other.Time);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return time.GetHashCode();
    }

    /// <summary>
    /// Determines whether two <see cref="LamportClockTimestamp"/> instances are not equal.
    /// </summary>
    public static bool operator !=(
        LamportClockTimestamp? left,
        LamportClockTimestamp? right)
        => !(left == right);

    /// <summary>
    /// Determines whether two <see cref="LamportClockTimestamp"/> instances are equal.
    /// </summary>
    public static bool operator ==(
        LamportClockTimestamp? left,
        LamportClockTimestamp? right)
    {
        if (left is null || right is null)
            return left is null && right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether one <see cref="LamportClockTimestamp"/> instance is greater than another.
    /// </summary>
    public static bool operator >(LamportClockTimestamp left, LamportClockTimestamp right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether one <see cref="LamportClockTimestamp"/> instance is less than another.
    /// </summary>
    public static bool operator <(LamportClockTimestamp left, LamportClockTimestamp right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether one <see cref="LamportClockTimestamp"/> instance is greater than or equal to another.
    /// </summary>
    public static bool operator >=(LamportClockTimestamp left, LamportClockTimestamp right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether one <see cref="LamportClockTimestamp"/> instance is less than or equal to another.
    /// </summary>
    public static bool operator <=(LamportClockTimestamp left, LamportClockTimestamp right)
        => left.CompareTo(right) <= 0;
}
