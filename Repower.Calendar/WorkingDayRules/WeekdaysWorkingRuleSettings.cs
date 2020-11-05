using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Repower.Calendar
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public partial class WeekdaysWorkingRuleSettings
    {
        [DataMember]
        public IEnumerable<DaySetting> Days { get; }
        public WeekdaysWorkingRuleSettings(IEnumerable<DaySetting> days)
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
