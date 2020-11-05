using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Repower.Calendar.Abstractions
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public sealed class CalendarDays: List<CalendarDaysEntry>
    {
        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar", Name ="Day")]
    public class CalendarDaysEntry
    {
        [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
        public class TimePeriod {
            public string Begin { get; set; }
            public string End { get; set; }
        }
        public string Date { get; set; }
        public string Description { get; set; }
        public bool IsWorkingDay { get; set; }
        public IEnumerable<TimePeriod> WorkingPeriods { get; set; }
    }
}
