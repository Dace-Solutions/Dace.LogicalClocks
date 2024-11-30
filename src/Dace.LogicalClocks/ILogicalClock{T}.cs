namespace Dace.LogicalClocks;

/// <summary>
/// Represents a generic logical clock interface that extends the <see cref="ILogicalClock"/> interface
/// and accepts a specific type of <see cref="ILogicalClockTimestamp"/>.
/// </summary>
/// <typeparam name="T">The type of the logical clock tick.</typeparam>
public interface ILogicalClock<T> : ILogicalClock
    where T : ILogicalClockTimestamp
{
    /// <inheritdoc />
    new ValueTask<T> TickAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the logical clock based on a received tick from another logical clock of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="received">The received logical clock tick of type <typeparamref name="T"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ValueTask" /> that represents the asynchronous save operation. The <see cref="ValueTask" /> result contains the current logical clock timestamp.</returns>
    ValueTask<T> WitnessAsync(
        T received,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    new ValueTask<T> CurrentAsync(
        CancellationToken cancellationToken = default);
}
