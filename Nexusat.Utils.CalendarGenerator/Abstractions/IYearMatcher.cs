namespace Nexusat.Utils.CalendarGenerator
{
    public interface IYearMatcher : IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneYear { get; }
    }
}