using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repower.Calendar
{
    public interface IWorkingDayInfoProvider
    {
        IWorkingDayInfo GetWorkingDayInfo(DateTime date);
    }
}
