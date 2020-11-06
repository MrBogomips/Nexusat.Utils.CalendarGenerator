using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nexusat.Calendar
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayInfo : IDayInfo
    {
        public bool IsWorkingDay { get; set; }

        public string Description { get; set; }

        public IEnumerable<TimePeriod> WorkingPeriods { get; set; }
    }
}
