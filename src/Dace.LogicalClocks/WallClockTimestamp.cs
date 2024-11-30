namespace Dace.LogicalClocks;

using System;
using System.Diagnostics.CodeAnalysis;

public readonly struct WallClockTimestamp : IComparable<WallClockTimestamp>,
    IComparable,
    IEquatable<WallClockTimestamp>
{
    public static WallClockTimestamp Zero { get; } = new WallClockTimestamp(DateTime.MinValue);
    public WallClockTimestamp(DateTime dateTime)
    {
        Time = dateTime;
    }

    public DateTime Time { get; }

    public int CompareTo(
        WallClockTimestamp other)
    {
        return Time.CompareTo(other.Time);
    }

    public override int GetHashCode()
    {
        return Time.GetHashCode();
    }

    public bool Equals(
        WallClockTimestamp other)
    {
        return Time == other.Time;
    }

    public override bool Equals(
        [NotNullWhen(true)] object? obj)
    {
        return obj is WallClockTimestamp value
            && Equals(value);
    }

    int IComparable.CompareTo(
        object? other)
    {
        return other is WallClockTimestamp wallClock
            ? CompareTo(wallClock)
            : throw new ArgumentException($"The provided value is not a {nameof(WallClockTimestamp)}.", nameof(other));
    }

    public static bool operator ==(WallClockTimestamp left, WallClockTimestamp right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(WallClockTimestamp left, WallClockTimestamp right)
    {
        return !(left == right);
    }

    public static bool operator <(WallClockTimestamp left, WallClockTimestamp right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(WallClockTimestamp left, WallClockTimestamp right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(WallClockTimestamp left, WallClockTimestamp right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(WallClockTimestamp left, WallClockTimestamp right)
    {
        return left.CompareTo(right) >= 0;
    }
}
