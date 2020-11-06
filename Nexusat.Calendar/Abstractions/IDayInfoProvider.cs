using System;

namespace Nexusat.Calendar
{
    public interface IDayInfoProvider
    {
        /// <summary>
        /// The implementation can return a <c>null</c> meaning no info available
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IDayInfo GetDayInfo(DateTime date);

        bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo);
    }
}
