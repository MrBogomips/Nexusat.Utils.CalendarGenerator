using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class CronDayRule: DayRule
    {
        public string Description { get; }
        public IReadOnlyList<IYearMatcher> YearMatchers { get; }
        public IReadOnlyList<MonthMatcher> MonthMatchers { get; }
        public IReadOnlyList<DayOfMonthMatcher> DayOfMonthMatchers { get; }
        public IReadOnlyList<DayOfWeekMatcher> DayOfWeekMatchers { get; }
        public IReadOnlyList<TimePeriod> WorkingPeriods { get; }

        public CronDayRule(
            string description,
            IReadOnlyList<IYearMatcher> yearMatchers, 
            IReadOnlyList<MonthMatcher> monthMatchers, 
            IReadOnlyList<DayOfMonthMatcher> dayOfMonthMatchers, 
            IReadOnlyList<DayOfWeekMatcher> dayOfWeekMatchers,
            IReadOnlyList<TimePeriod> workingPeriods
            )
        {
            Description = description;
            YearMatchers = yearMatchers;
            MonthMatchers = monthMatchers;
            DayOfMonthMatchers = dayOfMonthMatchers;
            DayOfWeekMatchers = dayOfWeekMatchers;
            WorkingPeriods = workingPeriods;
                
            DayInfo.NormalizeWorkingPeriods(workingPeriods);
        }

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
            throw new NotImplementedException();
        }
    }
}