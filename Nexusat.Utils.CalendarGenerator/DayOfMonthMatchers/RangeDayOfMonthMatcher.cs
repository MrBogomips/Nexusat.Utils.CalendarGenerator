using System;
using System.Diagnostics.CodeAnalysis;
using static Nexusat.Utils.CalendarGenerator.DayOfMonthMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Month matcher for a range of months
    /// </summary>
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class RangeDayOfMonthMatcher : RangeNumberMatcher, IDayOfMonthMatcher
    {
        public RangeDayOfMonthMatcher(int? left, int? right) : base(RangeCtorHelper(left, right))
        {
        }

        public virtual bool Match(DateTime date) => Match(date.Day);

        public bool IsOneDay => IsOneValue;

        public static bool TryParse(string value, out RangeDayOfMonthMatcher rangeDayOfMonthMatcher)
        {
            rangeDayOfMonthMatcher = default;
            if (!TryParse(value, 1, 31, 1, 31, out var left, out var right)) return false;

            rangeDayOfMonthMatcher = new RangeDayOfMonthMatcher(left, right);
            return true;
        }

        public override string ToString() => Left == 1 && Right == 31 ? "*" : base.ToString();
    }
}