using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Utils.CalendarGenerator
{
    public partial class WeekdaysWorkingRuleSettings
    {
        [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar", Name = nameof(DaySetting))]
        public class DaySetting
        {
            public DaySetting(System.DayOfWeek dayOfWeek, IEnumerable<TimePeriod> workingPeriods = null)
            {
                DayOfWeek = dayOfWeek;
                WorkingPeriods = workingPeriods == null ? new List<TimePeriod>() : new List<TimePeriod>(workingPeriods);
                if (!WorkingPeriods.Any())
                    WorkingPeriods.Add(TimePeriod.AllDay);
                else
                    DayInfo.NormalizeWorkingPeriods(WorkingPeriods);
            }

            [DataMember] public System.DayOfWeek DayOfWeek { get; private set; }

            [DataMember] public List<TimePeriod> WorkingPeriods { get; private set; }
        }
    }
}