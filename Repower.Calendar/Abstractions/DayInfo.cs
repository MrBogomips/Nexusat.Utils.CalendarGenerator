using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Repower.Calendar
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayInfo : IDayInfo
    {
        public bool IsWorkingDay { get; set; }

        public string Description { get; set; }

        public IList<TimePeriod> WorkingPeriods { get; set; }
    }
}
