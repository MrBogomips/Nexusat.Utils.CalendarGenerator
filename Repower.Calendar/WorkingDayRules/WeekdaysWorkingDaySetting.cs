using System;
using System.Collections.Generic;
using System.Linq;

namespace Repower.Calendar
{
    public partial class WeekdaysWorkingRuleSettings
    {
        public class DaySetting
        {
            public DayOfWeek DayOfWeek { get; }
            public List<TimePeriod> WorkingPeriods { get; }

            public DaySetting(DayOfWeek dayOfWeek, IEnumerable<TimePeriod> workingPeriods = null)
            {
                DayOfWeek = dayOfWeek;
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
        
    }
}
