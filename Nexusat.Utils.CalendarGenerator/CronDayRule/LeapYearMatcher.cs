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
        
        public static bool TryParse(string value, out LeapYearMatcher leapYearMatcherMatcher)
        {
            leapYearMatcherMatcher = default;
            if (value is null || !value.EndsWith("/Leap")) return false;
            var range = value.Remove(value.IndexOf("/Leap", StringComparison.Ordinal));
            if (!TryParse(range, out var left, out var right)) return false;
            if (left.HasValue && left == right) return false; // Single year range are invalid
            leapYearMatcherMatcher = new LeapYearMatcher(left, right);
            return true;
        }
    }
}