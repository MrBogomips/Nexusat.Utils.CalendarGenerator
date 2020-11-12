using System;
using static Nexusat.Utils.CalendarGenerator.MonthMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Month matcher expressing a period condition on the month value and subsequent steps
    /// </summary>
    public class PeriodicMonthMatcher : PeriodicNumberMatcher, IMonthMatcher
    {
        public PeriodicMonthMatcher(int left, int? right, int period) : base(PeriodicCtorHelper(left, right, period))
        {
        }

        public bool Match(DateTime date)
        {
            return Match(date.Month);
        }

        public bool IsOneMonth => IsOneValue;

        public static bool TryParse(string value, out PeriodicMonthMatcher periodicMonthMatcher)
        {
            periodicMonthMatcher = default;
            if (!TryParse(value, 1, 12, 1, 12, out var left, out var right, out var period)) return false;
            if (left < 1 || left > 12) return false;
            if (right.HasValue && (right.Value < 1 || right.Value > 12)) return false;

            periodicMonthMatcher = new PeriodicMonthMatcher(left, right, period);
            return true;
        }

        public override string ToString()
        {
            return Left == 1 && Right == 12 ? $"*/{Period}" : base.ToString();
        }
    }
}