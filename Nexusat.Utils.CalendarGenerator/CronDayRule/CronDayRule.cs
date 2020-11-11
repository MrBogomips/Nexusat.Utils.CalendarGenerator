using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class CronDayRule: DayRule
    {
        public string Description { get; }
        public IReadOnlyList<IYearMatcher> YearMatchers { get; }
        public IReadOnlyList<IMonthMatcher> MonthMatchers { get; }
        public IReadOnlyList<IDayOfMonthMatcher> DayOfMonthMatchers { get; }
        public IReadOnlyList<IDayOfWeekMatcher> DayOfWeekMatchers { get; }
        public IReadOnlyList<TimePeriod> WorkingPeriods { get; }

        public CronDayRule(
            string description,
            IEnumerable<IYearMatcher> yearMatchers, 
            IEnumerable<IMonthMatcher> monthMatchers, 
            IEnumerable<IDayOfMonthMatcher> dayOfMonthMatchers, 
            IEnumerable<IDayOfWeekMatcher> dayOfWeekMatchers,
            IEnumerable<TimePeriod> workingPeriods
            )
        {
            Description = description;
            YearMatchers = new List<IYearMatcher>(yearMatchers);
            MonthMatchers = new List<IMonthMatcher>(monthMatchers);
            DayOfMonthMatchers = new List<IDayOfMonthMatcher>(dayOfMonthMatchers);
            DayOfWeekMatchers = new List<IDayOfWeekMatcher>(dayOfWeekMatchers);
            WorkingPeriods = workingPeriods is not null ? new List<TimePeriod>(workingPeriods) : null;
                
            DayInfo.NormalizeWorkingPeriods(WorkingPeriods);
        }

        /// <summary>
        /// Returns a day info if the rule applyes
        /// </summary>
        /// <param name="date"></param>
        /// <returns>'null' in case the rule can't apply</returns>
        public override DayInfo GetDayInfo(DateTime date)
        {
            if (YearMatchers.Any(_ => _.Match(date)) &&
                MonthMatchers.Any(_ => _.Match(date)) &&
                DayOfMonthMatchers.Any(_ => _.Match(date)) &&
                DayOfWeekMatchers.Any(_ => _.Match(date)))
            {
                return new DayInfo(Description, WorkingPeriods);
            }
            return null;
        }

        public override bool TryGetDayInfo(DateTime date, out DayInfo dayInfo)
        {
            dayInfo = default;
            if (YearMatchers.Any(_ => _.Match(date)) &&
                MonthMatchers.Any(_ => _.Match(date)) &&
                DayOfMonthMatchers.Any(_ => _.Match(date)) &&
                DayOfWeekMatchers.Any(_ => _.Match(date)))
            {
                dayInfo = new DayInfo(Description, WorkingPeriods);
            }
            return dayInfo is not null;
        }
    }
}