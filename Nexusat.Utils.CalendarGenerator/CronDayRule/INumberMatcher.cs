namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface INumberMatcher
    {
        bool Match(int value);
    }
}