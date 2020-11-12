namespace Nexusat.Utils.CalendarGenerator
{
    public interface IDateMatcherParse<T> where T : IDateMatcher
    {
        bool TryParse(string value, out T dateMatcher);
    }
}