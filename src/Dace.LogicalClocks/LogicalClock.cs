namespace Dace.LogicalClocks;

public abstract class LogicalClock<T> : ILogicalClock<T>
    where T : ILogicalClockTimestamp
{
    protected LogicalClock()
    {
        
    }

    /// <summary>
    /// Gets the current timestamp of the clock.
    /// </summary>
    /// <returns>The current <see cref="T"/>.</returns>
    public abstract T Current();

    /// <summary>
    /// Updates the clock by witnessing a received timestamp from another clock.
    /// This ensures that the clock accounts for the received timestamp while maintaining
    /// the logical clock ordering constraints.
    /// </summary>
    /// <param name="receiveTimestamp">The received <see cref="T"/>.</param>
    public abstract T Witness(
        T receiveTimestamp);

    /// <summary>
    /// Advances the clock by one tick.
    /// </summary>
    public abstract T Tick();

    /// <inheritdoc/>
    ValueTask<T> ILogicalClock<T>.TickAsync(
        CancellationToken cancellationToken)
    {
        var value = Tick();
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<T> ILogicalClock<T>.CurrentAsync(
        CancellationToken cancellationToken)
    {
        var value = Current();
        return ValueTask.FromResult(value);
    }

    /// <inheritdoc/>
    ValueTask<T> ILogicalClock<T>.WitnessAsync(
        T received,
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
        if (received is T timestamp)
        {
            ILogicalClockTimestamp value = Witness(timestamp);
            return ValueTask.FromResult(value);
        }
        throw new ArgumentException($"The provided clock is not a {nameof(T)}.", nameof(received));
    }
}
