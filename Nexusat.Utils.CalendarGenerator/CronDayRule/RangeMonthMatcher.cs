using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Month matcher for a range of months
    /// </summary>
    public class RangeMonthMatcher : RangeNumberMatcher, IMonthMatcher
    {
        public RangeMonthMatcher(int? left, int? right) : base(left, right)
        {
            if (left.HasValue && (left.Value < 1 || left.Value > 12))
                throw new ArgumentOutOfRangeException(nameof(left), left.Value, "Must be between 1 and 12");
            if (right.HasValue && (right.Value < 1 || right.Value > 12))
                throw new ArgumentOutOfRangeException(nameof(right), right.Value, "Must be between 1 and 12");
        }

        public virtual bool Match(DateTime date) => Match(date.Month);
        public bool IsOneMonth => IsOneValue;

        public static bool TryParse(string value, out RangeMonthMatcher rangeMonthMatcher)
        {
            rangeMonthMatcher = default;
            if (!TryParse(value, out var left, out var right)) return false;
            if (left.HasValue && (left.Value < 1 || left.Value > 12)) return false;
            if (right.HasValue && (right.Value < 1 || right.Value > 12)) return false;

            rangeMonthMatcher = new RangeMonthMatcher(left, right);
            return true;
        }
    }
}