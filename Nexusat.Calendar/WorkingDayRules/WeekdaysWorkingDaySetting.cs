using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Calendar
{
    public partial class WeekdaysWorkingRuleSettings
    {
        [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
        public class DaySetting
        {
            [DataMember]
            public DayOfWeek DayOfWeek { get; private set; }
            [DataMember]
            public List<TimePeriod> WorkingPeriods { get; private set; }

            public DaySetting(DayOfWeek dayOfWeek, IEnumerable<TimePeriod> workingPeriods = null)
            {
                DayOfWeek = dayOfWeek;
                WorkingPeriods = workingPeriods == null ? new List<TimePeriod>() : new List<TimePeriod>(workingPeriods);
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
        
    }
}
