using System;
using static Nexusat.Utils.CalendarGenerator.MonthMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Month matcher for a range of months
    /// </summary>
    public class RangeMonthMatcher : RangeNumberMatcher, IMonthMatcher
    {
        public RangeMonthMatcher(int? left, int? right) : base(RangeCtorHelper(left, right))
        {
        }

        public virtual bool Match(DateTime date)
        {
            return Match(date.Month);
        }

        public bool IsOneMonth => IsOneValue;

        public static bool TryParse(string value, out RangeMonthMatcher rangeMonthMatcher)
        {
            rangeMonthMatcher = default;
            if (!TryParse(value, 1, 12, 1, 12, out var left, out var right)) return false;

            rangeMonthMatcher = new RangeMonthMatcher(left, right);
            return true;
        }

        public override string ToString()
        {
            return Left == 1 && Right == 12 ? "*" : base.ToString();
        }
    }
}