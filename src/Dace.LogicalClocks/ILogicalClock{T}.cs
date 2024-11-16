namespace Dace.LogicalClocks;
public interface ILogicalClock<T> : ILogicalClock
    where T : ILogicalClockTick
{
    void Witness(T received);
    new T Current();
}
