namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IYearMatcher: IDateMatcher, IRangeNumberMatcher
    {
        bool IsOneYear { get; }
    }
}