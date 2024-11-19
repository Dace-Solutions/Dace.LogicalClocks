namespace Dace.LogicalClocks;

/// <summary>
/// Represents a logical clock interface that provides methods for managing and querying logical clock.
/// </summary>
public interface ILogicalClock
{
    /// <summary>
    /// Advances the logical clock by one tick.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ValueTask" /> that represents the asynchronous save operation. The <see cref="ValueTask" /> result contains the current logical clock timestamp.</returns>
    ValueTask<ILogicalClockTimestamp> TickAsync(
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates the logical clock based on a received timestamp from another logical clock.
    /// </summary>
    /// <param name="received">The received logical clock timestamp.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ValueTask" /> that represents the asynchronous save operation. The <see cref="ValueTask" /> result contains the current logical clock timestamp.</returns>
    ValueTask<ILogicalClockTimestamp> WitnessAsync(
        ILogicalClockTimestamp received,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the current tick of the logical clock.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ValueTask" /> that represents the asynchronous save operation. The <see cref="ValueTask" /> result contains the current logical clock timestamp.</returns>
    ValueTask<ILogicalClockTimestamp> CurrentAsync(
        CancellationToken cancellationToken = default);
}
