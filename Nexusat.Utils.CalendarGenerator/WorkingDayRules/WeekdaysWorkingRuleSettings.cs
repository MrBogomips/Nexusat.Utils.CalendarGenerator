using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation.Possible

namespace Nexusat.Utils.CalendarGenerator
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public partial class WeekdaysWorkingRuleSettings
    {
        [DataMember]
        public IEnumerable<DaySetting> Days { get; private set; }
        public WeekdaysWorkingRuleSettings(IEnumerable<DaySetting> days)
        {
            Days = days ?? throw new ArgumentNullException(nameof(days));
            
            var daysProcessed = new List<DayOfWeek>();
            foreach (var d in days)
            {
                // ASSERT: Weekday MUST appear once
                if (daysProcessed.Contains(d.DayOfWeek))
                {
                    throw new ArgumentException($"Day of week {d.DayOfWeek} appears more than once", nameof(days));
                }
                daysProcessed.Add(d.DayOfWeek);
                // ASSERT: Working periods can't overlap
                // PRECONDITION d.WorkingPeriods.Sort(); already sorted
                // PRECONDITION d.WorkingPeriods has at least one element
                var prevTimePeriod = d.WorkingPeriods.First();
                for (var i = 1; i < d.WorkingPeriods.Count; i++)
                {
                    if (prevTimePeriod.Overlaps(d.WorkingPeriods[i])) throw new ArgumentException("Working Periods can't overlap");
                }
            }
        }
        
    }
}
