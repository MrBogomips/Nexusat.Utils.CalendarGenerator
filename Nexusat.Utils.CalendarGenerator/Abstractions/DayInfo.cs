using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    // TODO: Classe Working Periods con Parse per stringhe del tipo "08:00-12:00 13:00-17:00
    //       La validazione dei working Periods non sovrapposti viene fatta qui
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayInfo : IDayInfo
    {
        [field: DataMember]
        public bool IsWorkingDay { get; }

        [field: DataMember]
        public string Description { get; }

        [field: DataMember]
        public IEnumerable<TimePeriod> WorkingPeriods { get; }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public DayInfo(string description = null, IEnumerable<TimePeriod> workingPeriods = null)
        {
            Description = description;
            WorkingPeriods = workingPeriods;
            if (workingPeriods is not null && !workingPeriods.Any())
                WorkingPeriods = null; // force null in case of empty working periods
            IsWorkingDay = WorkingPeriods is not null;
        }
    }
}
