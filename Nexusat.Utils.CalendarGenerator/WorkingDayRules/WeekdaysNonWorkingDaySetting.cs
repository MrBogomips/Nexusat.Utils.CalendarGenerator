using System;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public partial class WeekdaysNonWorkingRuleSettings
    {
        [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
        public class DaySetting
        {
            public DaySetting(DayOfWeek dayOfWeek)
            {
                DayOfWeek = dayOfWeek;
            }

            [DataMember] public DayOfWeek DayOfWeek { get; private set; }
        }
    }
}