using System;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    [KnownType(typeof(WeekdaysWorkingRule))]
    [KnownType(typeof(WeekdaysNonWorkingRule))]
    public abstract class DayRule : IDayInfoProvider
    {
        public abstract DayInfo GetDayInfo(DateTime date);

        public abstract bool TryGetDayInfo(DateTime date, out DayInfo dayInfo);
    }
}