using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Repower.Calendar
{

    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    [KnownType(typeof(WeekdaysWorkingRule)), KnownType(typeof(WeekdaysNonWorkingRule))]
    public abstract class DayRule: IDayInfoProvider
    {
        public abstract IDayInfo GetDayInfo(DateTime date);

        public abstract bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo);
    }
}
