namespace Dace.LogicalClocks;

using System;
using System.Diagnostics.CodeAnalysis;

public readonly struct WallClockTimestamp(double unixEpoch) : IComparable<WallClockTimestamp>,
    IComparable,
    IEquatable<WallClockTimestamp>
{
    public static WallClockTimestamp Zero { get; } = new(DateTime.MinValue);

    public double UnixEpoch { get; } = unixEpoch;

    public WallClockTimestamp(DateTime dateTime)
        : this((dateTime - DateTime.UnixEpoch).TotalNanoseconds)
    {
    }

    public int CompareTo(
        WallClockTimestamp other)
    {
        return UnixEpoch.CompareTo(other.UnixEpoch);
    }

    public override int GetHashCode()
    {
        return UnixEpoch.GetHashCode();
    }

    public bool Equals(
        WallClockTimestamp other)
    {
        return UnixEpoch == other.UnixEpoch;
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

    public override string ToString()
    {
        return $"UnixEpoch: {UnixEpoch}";
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
