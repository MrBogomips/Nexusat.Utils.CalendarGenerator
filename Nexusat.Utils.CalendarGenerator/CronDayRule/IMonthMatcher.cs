namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IMonthMatcher: IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneMonth { get; }
    }
}