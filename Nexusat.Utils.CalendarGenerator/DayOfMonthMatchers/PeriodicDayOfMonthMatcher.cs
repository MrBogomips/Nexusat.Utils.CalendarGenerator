using System;
using static Nexusat.Utils.CalendarGenerator.DayOfMonthMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Day of month matcher expressing a period condition on the day value and subsequent steps
    /// </summary>
    public class PeriodicDayOfMonthMatcher : PeriodicNumberMatcher, IDayOfMonthMatcher
    {
        public PeriodicDayOfMonthMatcher(int left, int? right, int period) : base(
            PeriodicCtorHelper(left, right, period))
        {
        }

        public bool Match(DateTime date) => Match(date.Day);

        public bool IsOneDay => IsOneValue;

        public static bool TryParse(string value, out PeriodicDayOfMonthMatcher periodicMonthMatcher)
        {
            periodicMonthMatcher = default;
            if (!TryParse(value, 1, 31, 1, 31, out var left, out var right, out var period)) return false;
            if (left < 1 || left > 31) return false;
            if (right.HasValue && (right.Value < 1 || right.Value > 31)) return false;

            periodicMonthMatcher = new PeriodicDayOfMonthMatcher(left, right, period);
            return true;
        }

        public override string ToString() => Left == 1 && Right == 31 ? $"*/{Period}" : base.ToString();
    }
}