using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation.Possible

namespace Nexusat.Utils.CalendarGenerator
{
    public partial class WeekdaysNonWorkingRuleSettings
    {
        public WeekdaysNonWorkingRuleSettings(IEnumerable<DaySetting> days)
        {
            Days = days ?? throw new ArgumentNullException(nameof(days));

            var daysProcessed = new List<DayOfWeek>();
            foreach (var d in days)
            {
                // ASSERT: Weekday MUST appear once
                if (daysProcessed.Contains(d.DayOfWeek))
                    throw new ArgumentException($"Day of week {d.DayOfWeek} appears more than once", nameof(days));
                daysProcessed.Add(d.DayOfWeek);
            }
        }

        [DataMember] public IEnumerable<DaySetting> Days { get; private set; }
    }
}