using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public partial class WeekdaysNonWorkingRuleSettings
    {
        [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar", Name = nameof(DaySetting))]
        public class DaySetting
        {
            public DaySetting(System.DayOfWeek dayOfWeek)
            {
                DayOfWeek = dayOfWeek;
            }

            [DataMember] public System.DayOfWeek DayOfWeek { get; private set; }
        }
    }
}