namespace Dace.LogicalClock;

using Dace.LogicalClocks;
using System.Diagnostics.CodeAnalysis;

public partial class LamportClock
{
    public bool Equals(LamportClock? x, LamportClock? y)
        => x is null && y is null || x is not null && y is not null && x._clock == y._clock;

    public int GetHashCode([DisallowNull] LamportClock obj)
        => obj._clock.GetHashCode();

    public bool Equals(LamportClock? other)
        => other is not null && _clock == other._clock;

    int IComparable<ILogicalClock>.CompareTo(ILogicalClock? other)
    {
        if (other is null)
            return 1; // Null is considered less than any valid clock
        if (other is LamportClock lamportClock)
            return CompareTo(lamportClock);
        throw new ArgumentException($"The provided clock is not a {nameof(LamportClock)}.", nameof(other));
    }

    int IComparable.CompareTo(object? other)
        => other is LamportClock lamportClock ? CompareTo(lamportClock) : 1;

    bool IEqualityComparer<ILogicalClock>.Equals(ILogicalClock? x, ILogicalClock? y)
        => x is LamportClock lamportClock1 && y is LamportClock lamportClock2 ? Equals(lamportClock1, lamportClock2) : false;

    int IEqualityComparer<ILogicalClock>.GetHashCode(ILogicalClock obj)
        => obj is LamportClock lamportClock ? lamportClock.GetHashCode() : 1;

    bool IEquatable<ILogicalClock>.Equals(ILogicalClock? other)
        => other is LamportClock lamportClock ? Equals(lamportClock) : false;

    public override bool Equals(object? obj)
        => obj is LamportClock lamportClock && Equals(lamportClock);

    public int CompareTo(LamportClock? other)
        => other is null ? 1 : _clock.CompareTo(other._clock);

    public override int GetHashCode()
        => _clock.GetHashCode();

    public static bool operator !=(LamportClock? left, LamportClock? right)
        => !(left == right);

    public static bool operator ==(LamportClock? left, LamportClock? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator >(LamportClock left, LamportClock right)
        => left._clock > right._clock;

    public static bool operator <(LamportClock left, LamportClock right)
        => left._clock < right._clock;

    public static bool operator >=(LamportClock left, LamportClock right)
        => left._clock >= right._clock;

    public static bool operator <=(LamportClock left, LamportClock right)
        => left._clock <= right._clock;
}
