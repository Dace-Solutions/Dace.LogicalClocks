namespace Dace.LogicalClocks.Lamport;
public partial class LamportClock
{
    /// <inheritdoc/>
    ValueTask<LamportClockTimestamp> ILogicalClock<LamportClockTimestamp>.TickAsync(
        CancellationToken cancellationToken)
    {
        var value = Current();
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<LamportClockTimestamp> ILogicalClock<LamportClockTimestamp>.CurrentAsync(
        CancellationToken cancellationToken)
    {
        var value = Current();
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<LamportClockTimestamp> ILogicalClock<LamportClockTimestamp>.WitnessAsync(
        LamportClockTimestamp received,
        CancellationToken cancellationToken)
    {
        var value = Witness(received);
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<ILogicalClockTimestamp> ILogicalClock.TickAsync(
        CancellationToken cancellationToken)
    {
        ILogicalClockTimestamp value = Tick();
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<ILogicalClockTimestamp> ILogicalClock.CurrentAsync(
        CancellationToken cancellationToken)
    {
        ILogicalClockTimestamp value = Current();
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<ILogicalClockTimestamp> ILogicalClock.WitnessAsync(
        ILogicalClockTimestamp received,
        CancellationToken cancellationToken)
    {
        ILogicalClockTimestamp value = Witness((LamportClockTimestamp)received);
        return ValueTask.FromResult(value);
    }
}
