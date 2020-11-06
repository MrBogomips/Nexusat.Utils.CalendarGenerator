using System;
using System.Runtime.Serialization;

namespace Nexusat.Calendar
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public partial class WeekdaysNonWorkingRuleSettings
    {
        [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
        public class DaySetting
        {
            [DataMember]
            public DayOfWeek DayOfWeek { get; private set; }

            public DaySetting(DayOfWeek dayOfWeek) => DayOfWeek = dayOfWeek;
        }

    }
}
