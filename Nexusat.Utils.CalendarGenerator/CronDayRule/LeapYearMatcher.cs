using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Year matcher for leap years
    /// </summary>
    public class LeapYearMatcher: RangeYearMatcher
    {
        public LeapYearMatcher(int? left, int? right) : base(left, right)
        {
            if (IsOneYear) throw new ArgumentException("You must specify a multi year range");
        }
        
        internal static bool IsLeapYear(DateTime date) 
            => date.Year % 4 == 0 && (date.Year % 100 != 0 || date.Year % 400 == 0);

        public override bool Match(DateTime date) => base.Match(date) && IsLeapYear(date);
         
        public override string ToString() => $"{base.ToString()}/Leap";
    }
}