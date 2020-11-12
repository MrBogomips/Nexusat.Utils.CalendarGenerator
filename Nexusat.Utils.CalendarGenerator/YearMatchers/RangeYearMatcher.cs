using System;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Year matcher for a range of years
    /// </summary>
    public class RangeYearMatcher : RangeNumberMatcher, IYearMatcher
    {
        public RangeYearMatcher(int? left, int? right) : base(left, right)
        {
        }

        public virtual bool Match(DateTime date)
        {
            return Match(date.Year);
        }

        public bool IsOneYear => IsOneValue;

        public static bool TryParse(string value, out RangeYearMatcher rangeNumberMatcher)
        {
            if (!TryParse(value, null, null, null, null, out var left, out var right))
            {
                rangeNumberMatcher = null;
                return false;
            }

            rangeNumberMatcher = new RangeYearMatcher(left, right);
            return true;
        }
    }
}