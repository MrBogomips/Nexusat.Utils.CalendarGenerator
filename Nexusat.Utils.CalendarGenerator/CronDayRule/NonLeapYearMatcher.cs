using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Year matcher for non-leap years
    /// </summary>
    public class NonLeapYearMatcher : RangeYearMatcher
    {
        public NonLeapYearMatcher(int? left, int? right) : base(left, right)
        {
            if (IsOneYear) throw new ArgumentException("You must specify a multi year range");
        }
        
        public override bool Match(DateTime date) => base.Match(date) && !LeapYearMatcher.IsLeapYear(date);
        public override string ToString() => $"{base.ToString()}/NotLeap";
    }
}