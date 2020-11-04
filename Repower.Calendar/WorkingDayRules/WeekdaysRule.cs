using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repower.Calendar
{
    public class WeekdayRuleSettings
    {
        public class DaySetting
        {
            public DayOfWeek DayOfWeek { get; set; }
            public List<TimePeriod> WorkingPeriods { get; set; }
        }
        public IEnumerable<DaySetting> Days { get; }
        public WeekdayRuleSettings(IEnumerable<DaySetting> days)
        {
            Days = days ?? throw new ArgumentNullException(nameof(days));
            
            List<DayOfWeek> daysProcessed = new List<DayOfWeek>();
            foreach (var d in days)
            {
                // ASSERT: Weekday MUST appear once
                if (daysProcessed.Contains(d.DayOfWeek))
                {
                    throw new ArgumentException($"Day of week ${d.DayOfWeek} appears more than once", nameof(days));
                }
                daysProcessed.Add(d.DayOfWeek);
                // ASSERT: Working periods can't overlap
                //List<TimePeriod> workingPeriod = d.WorkingPeriods.Sort((TimePeriod lhs, WorkingPeriod))

            }
        }
        
    }

    public class WeekdaysRule : IWorkingDayRule
    {
        internal static readonly string NAME = "Weekday";
        public string Name => NAME;

        public IWorkingDayInfo GetWorkingDayInfo(DateTime date)
        {
            IWorkingDayInfo dayInfo = new WorkingDayInfo();

            return dayInfo;
        }

    }
}
