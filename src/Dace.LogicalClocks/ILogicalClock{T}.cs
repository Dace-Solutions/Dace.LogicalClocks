namespace Dace.LogicalClocks;

/// <summary>
/// Represents a generic logical clock interface that extends the <see cref="ILogicalClock"/> interface
/// and accepts a specific type of <see cref="ILogicalClockTimestamp"/>.
/// </summary>
/// <typeparam name="T">The type of the logical clock tick.</typeparam>
public interface ILogicalClock<T> : ILogicalClock
    where T : ILogicalClockTimestamp
{
    /// <summary>
    /// Updates the logical clock based on a received tick from another logical clock of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="received">The received logical clock tick of type <typeparamref name="T"/>.</param>
    void Witness(T received);

    /// <summary>
    /// Gets the current tick of the logical clock of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The current logical clock tick of type <typeparamref name="T"/>.</returns>
    new T Current();
}
