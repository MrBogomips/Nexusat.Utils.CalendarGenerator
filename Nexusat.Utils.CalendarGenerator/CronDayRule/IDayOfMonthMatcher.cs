namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IDayOfMonthMatcher: IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneDay { get; }
    }
}