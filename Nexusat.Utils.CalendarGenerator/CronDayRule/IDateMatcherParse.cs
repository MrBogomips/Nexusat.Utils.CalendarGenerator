namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IDateMatcherParse<T> where T : IDateMatcher
    {
        bool TryParse(string value, out T dateMatcher);
    }
}