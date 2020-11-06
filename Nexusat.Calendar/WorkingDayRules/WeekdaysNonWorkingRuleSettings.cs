using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nexusat.Calendar
{
    public partial class WeekdaysNonWorkingRuleSettings
    {
        [DataMember]
        public IEnumerable<DaySetting> Days { get; private set; }
        public WeekdaysNonWorkingRuleSettings(IEnumerable<DaySetting> days)
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
            }
        }

    }
}
