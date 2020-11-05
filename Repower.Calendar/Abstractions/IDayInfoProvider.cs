using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repower.Calendar
{
    public interface IDayInfoProvider
    {
        /// <summary>
        /// The impleemtation can return a <c>null</c> meaning no info available
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IDayInfo GetDayInfo(DateTime date);

        bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo);
    }
}
