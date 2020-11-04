using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repower.Calendar
{
    public interface IWorkingDayInfoProvider
    {
        /// <summary>
        /// The impleemtation can return a <c>null</c> meaning no info available
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IWorkingDayInfo GetWorkingDayInfo(DateTime date);
    }
}
