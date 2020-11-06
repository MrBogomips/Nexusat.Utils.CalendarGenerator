using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    // TODO: semplificare
    //       IsWorking day è True se i WOrking Peiods sono non vuoti
    //       La validazione dei working Periods non sovrapposti viene fatta qui
    // TODO: Classe Working Periods con Parse per stringhe del tipo "08:00-12:00 13:00-17:00
    //       La validazione dei working Periods non sovrapposti viene fatta qui
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayInfo : IDayInfo
    {
        public bool IsWorkingDay { get; set; }

        public string Description { get; set; }

        public IEnumerable<TimePeriod> WorkingPeriods { get; set; }
    }
}
