namespace Nexusat.Utils.CalendarGenerator
{
    public abstract class NumberMatcher : INumberMatcher
    {
        public abstract bool Match(int value);
    }
}