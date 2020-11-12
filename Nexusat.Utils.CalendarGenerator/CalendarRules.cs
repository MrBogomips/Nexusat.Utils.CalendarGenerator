using System;
using System.Collections.Generic;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     A chain of <see cref="DayRule" />s to manage the working days
    /// </summary>
    //[DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class CalendarRules : List<CalendarRule>
    {
        public void Add(DayRulePolicy policy, DayRule rule)
        {
            Add(new CalendarRule(policy, rule));
        }

        public void Add(DayRulePolicy policy,
            string description,
            IEnumerable<IYearMatcher> yearMatchers,
            IEnumerable<IMonthMatcher> monthMatchers,
            IEnumerable<IDayOfMonthMatcher> dayOfMonthMatchers,
            IEnumerable<IDayOfWeekMatcher> dayOfWeekMatchers,
            IEnumerable<TimePeriod> workingPeriods
        ) => Add(policy,
            new DayRule(description, yearMatchers, monthMatchers, dayOfMonthMatchers, dayOfWeekMatchers,
                workingPeriods));

        /// <summary>
        ///     Add the rules from the calendar to the collection of rules
        /// </summary>
        /// <param name="calendar"></param>
        public void Add(Calendar calendar)
        {
            if (calendar == null) throw new ArgumentNullException(nameof(calendar));
            AddRange(calendar.CalendarRules);
        }
    }
}