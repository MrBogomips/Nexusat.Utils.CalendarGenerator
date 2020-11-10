namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IDayOfWeekMatcher: IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneWeekDay { get; }
    }
}