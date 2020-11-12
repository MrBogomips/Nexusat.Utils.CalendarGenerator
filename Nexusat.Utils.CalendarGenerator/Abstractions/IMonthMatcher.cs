namespace Nexusat.Utils.CalendarGenerator
{
    public interface IMonthMatcher : IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneMonth { get; }
    }
}