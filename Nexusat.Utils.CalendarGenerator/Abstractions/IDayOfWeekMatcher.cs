namespace Nexusat.Utils.CalendarGenerator
{
    public interface IDayOfWeekMatcher : IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneWeekDay { get; }
    }
}