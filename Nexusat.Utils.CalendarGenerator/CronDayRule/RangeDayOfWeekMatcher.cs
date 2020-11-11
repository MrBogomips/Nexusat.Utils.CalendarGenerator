using System;
using static Nexusat.Utils.CalendarGenerator.CronDayRule.DayOfWeekMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Day of week matcher for a range of week days.
    ///
    /// Week days are numbered as follow:
    /// Sun: 0 or 7
    /// Mon: 1
    /// Tue: 2
    /// Wed: 3
    /// Thu: 4
    /// Fri: 5
    /// Sat: 6
    /// </summary>
    public class RangeDayOfWeekMatcher : RangeNumberMatcher, IDayOfWeekMatcher
    {
        public RangeDayOfWeekMatcher(int? left, int? right) : base(RangeCtorHelper(left, right))
        {
            
        }

        public virtual bool Match(DateTime date) => Match((int)date.DayOfWeek % 7);
        public bool IsOneWeekDay => IsOneValue;

        public static bool TryParse(string value, out RangeDayOfWeekMatcher rangeMonthMatcher)
        {
            rangeMonthMatcher = default;
            if (!TryParse(value, 0, 7, 0, 7, out var left, out var right)) return false;

            rangeMonthMatcher = new RangeDayOfWeekMatcher(left, right);
            return true;
        }

        public override string ToString() 
            => (Left, Right) == (1, 7)
               || (Left, Right) == (0, 7)
               || (Left, Right) == (1, 6)? $"*" : base.ToString();
    }
}