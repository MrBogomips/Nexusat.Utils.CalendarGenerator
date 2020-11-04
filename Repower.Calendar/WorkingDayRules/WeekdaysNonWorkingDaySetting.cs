using System;

namespace Repower.Calendar
{
    public partial class WeekdaysNonWorkingRuleSettings
    {
        public class DaySetting
        {
            public DayOfWeek DayOfWeek { get; }

            public DaySetting(DayOfWeek dayOfWeek) => DayOfWeek = dayOfWeek;
        }

    }
}
