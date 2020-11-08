using System;

// ReSharper disable UnusedMemberInSuper.Global

namespace Nexusat.Utils.CalendarGenerator
{
    public interface IDayInfoProvider
    {
        /// <summary>
        ///     The implementation can return a <c>null</c> meaning no info available
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        DayInfo GetDayInfo(DateTime date);

        bool TryGetDayInfo(DateTime date, out DayInfo dayInfo);
    }
}