namespace Nexusat.Utils.CalendarGenerator
{
    public interface IDayOfMonthMatcher : IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneDay { get; }
    }
}