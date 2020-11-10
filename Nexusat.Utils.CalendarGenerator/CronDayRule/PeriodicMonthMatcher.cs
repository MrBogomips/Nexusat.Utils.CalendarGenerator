using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Month matcher expressing a period condition on the year value and subsequent steps
    /// </summary>
    public class PeriodicMonthMatcher : PeriodicNumberMatcher, IMonthMatcher
    {
        public PeriodicMonthMatcher(int left, int? right, int period) : base(left, right, period)
        {
            if (left < 1 || left > 12)
                throw new ArgumentOutOfRangeException(nameof(left), left, "Must be between 1 and 12");
            if (right.HasValue && (right.Value < 1 || right.Value > 12))
                throw new ArgumentOutOfRangeException(nameof(right), right.Value, "Must be between 1 and 12");
        }

        public bool Match(DateTime date) => Match(date.Month);

        public bool IsOneMonth => IsOneValue;
        
        public static bool TryParse(string value, out PeriodicMonthMatcher periodicMonthMatcher)
        {
            periodicMonthMatcher = default;
            if (!TryParse(value, out var left, out var right, out var period)) return false;
            if (left < 1 || left > 12) return false;
            if (right.HasValue && (right.Value < 1 || right.Value > 12)) return false;
            
            periodicMonthMatcher = new PeriodicMonthMatcher(left, right, period);
            return true;
        }
    }
}