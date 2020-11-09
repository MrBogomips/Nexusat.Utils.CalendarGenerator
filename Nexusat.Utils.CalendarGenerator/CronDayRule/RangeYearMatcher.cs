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
    }
}