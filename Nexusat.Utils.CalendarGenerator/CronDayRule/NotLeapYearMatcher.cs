using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Year matcher for non-leap years
    /// </summary>
    public class NotLeapYearMatcher : RangeYearMatcher
    {
        public NotLeapYearMatcher(int? left, int? right) : base(left, right)
        {
            if (IsOneYear) throw new ArgumentException("You must specify a multi year range");
        }
        
        public override bool Match(DateTime date) => base.Match(date) && !LeapYearMatcher.IsLeapYear(date);
        public override string ToString() => $"{base.ToString()}/NotLeap";
        
        public static bool TryParse(string value, out NotLeapYearMatcher nonLeapYearMatcherMatcher)
        {
            nonLeapYearMatcherMatcher = default;
            if (value is null || !value.EndsWith("/NotLeap")) return false;
            var range = value.Remove(value.IndexOf("/NotLeap", StringComparison.Ordinal));
            if (!TryParse(range, out var left, out var right)) return false;
            if (left.HasValue && left == right) return false; // Single year range are invalid
            nonLeapYearMatcherMatcher = new NotLeapYearMatcher(left, right);
            return true;
        }
    }
}