namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public abstract class NumberMatcher : INumberMatcher
    {
        public abstract bool Match(int value);
    }
}