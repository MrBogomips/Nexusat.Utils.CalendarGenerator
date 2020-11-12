using System;
using static Nexusat.Utils.CalendarGenerator.DayOfWeekMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Day of week matcher for a range of week days.
    ///     Week days are numbered as follow:
    ///     Sun: 0 or 7
    ///     Mon: 1
    ///     Tue: 2
    ///     Wed: 3
    ///     Thu: 4
    ///     Fri: 5
    ///     Sat: 6
    /// </summary>
    public class RangeDayOfWeekMatcher : RangeNumberMatcher, IDayOfWeekMatcher
    {
        public RangeDayOfWeekMatcher(int? left, int? right) : base(RangeCtorHelper(left, right))
        {
        }

        public virtual bool Match(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Sunday => base.Match(0) || base.Match(7),
                var dow => base.Match((int) dow)
            };
        }

        public bool IsOneWeekDay => IsOneValue;

        public static bool TryParse(string value, out RangeDayOfWeekMatcher rangeMonthMatcher)
        {
            rangeMonthMatcher = default;
            if (!TryParse(value, 0, 7, 0, 7, out var left, out var right)) return false;

            rangeMonthMatcher = new RangeDayOfWeekMatcher(left, right);
            return true;
        }

        public override string ToString()
        {
            return (Left, Right) == (1, 7)
                   || (Left, Right) == (0, 7)
                   || (Left, Right) == (0, 6)
                   || (Left, Right) == (1, 6)
                ? "*"
                : base.ToString();
        }
    }
}