using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Year matcher for a range of years
    /// </summary>
    public class RangeYearMatcher : RangeNumberMatcher, IYearMatcher
    {
        public RangeYearMatcher(int? left, int? right) : base(left, right)
        {
        }

        public virtual bool Match(DateTime date) => Match(date.Year);
        public bool IsOneYear => IsOneValue;

        public static bool TryParse(string value, out RangeYearMatcher rangeNumberMatcher)
        {
            if (!RangeNumberMatcher.TryParse(value, out var left, out var right))
            {
                rangeNumberMatcher = null;
                return false;
            };
            rangeNumberMatcher = new RangeYearMatcher(left, right);
            return true;
        }
    }
}