using System;
using System.Collections.Generic;
using System.Linq;

namespace Repower.Calendar
{
    [Serializable]
    public class WeekdayRuleSettings
    {
        public class DaySetting
        {
            public DayOfWeek DayOfWeek { get; }
            public List<TimePeriod> WorkingPeriods { get; }

            public DaySetting(DayOfWeek dayOfWeek, IEnumerable<TimePeriod> workingPeriods = null)
            {
                DayOfWeek = DayOfWeek;
                if (workingPeriods == null)
                {
                    WorkingPeriods = new List<TimePeriod>();
                }
                else
                {
                    WorkingPeriods = new List<TimePeriod>(workingPeriods);
                }
                if (!WorkingPeriods.Any())
                {
                    WorkingPeriods.Add(TimePeriod.AllDay);
                }
                else
                {
                    WorkingPeriods.Sort();
                }
            }
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
                    throw new ArgumentException($"Day of week {d.DayOfWeek} appears more than once", nameof(days));
                }
                daysProcessed.Add(d.DayOfWeek);
                // ASSERT: Working periods can't overlap
                // PREREQ d.WorkingPeriods.Sort(); already sorted
                // PREREQ d.WorkingPeriods has at least one element
                TimePeriod prevTimePeriod = d.WorkingPeriods.First();
                for (int i = 1; i < d.WorkingPeriods.Count; i++)
                {
                    if (prevTimePeriod.Overlaps(d.WorkingPeriods[i])) throw new ArgumentException("Working Periods can't overlap");
                }
            }
        }
        
    }
}
