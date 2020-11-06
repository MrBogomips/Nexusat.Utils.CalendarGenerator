﻿using System;
using System.Runtime.Serialization;

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
