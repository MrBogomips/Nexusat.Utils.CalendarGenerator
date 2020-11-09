using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Year matcher expressing a period condition on the year value and subsequent steps
    /// </summary>
    public class PeriodicYearMatcher : PeriodicNumberMatcher, IYearMatcher
    {
        public PeriodicYearMatcher(int left, int? right, int period) : base(left, right, period)
        {
        }

        public bool Match(DateTime date) => Match(date.Year);

        public bool IsOneYear => IsOneValue;
    }
}