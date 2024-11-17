namespace Dace.LogicalClocks;

/// <summary>
/// Represents a logical clock interface that provides methods for managing and querying logical clock.
/// </summary>
public interface ILogicalClock
{
    /// <summary>
    /// Advances the logical clock by one tick.
    /// </summary>
    void Tick();
    /// <summary>
    /// Updates the logical clock based on a received timestamp from another logical clock.
    /// </summary>
    /// <param name="received">The received logical clock timestamp.</param>
    void Witness(ILogicalClockTimestamp received);
    /// <summary>
    /// Gets the current tick of the logical clock.
    /// </summary>
    /// <returns>The current logical clock timestamp.</returns>
    ILogicalClockTimestamp Current();
}
