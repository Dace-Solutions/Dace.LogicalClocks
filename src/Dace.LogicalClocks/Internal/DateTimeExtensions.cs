namespace Dace.LogicalClocks.Internal;
internal static class DateTimeExtensions
{
    public static double GetTotalNanoseconds(this DateTime dateTime)
    {
        return TimeSpan.FromTicks(dateTime.Ticks).TotalNanoseconds;
    }
}
