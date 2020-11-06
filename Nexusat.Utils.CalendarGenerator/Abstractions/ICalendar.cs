using System;
// ReSharper disable UnusedMemberInSuper.Global

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    /// Abstraction of a Calendar
    /// </summary>
    public interface ICalendar: IDayInfoProvider
    {
        /// <summary>
        /// The name of the calendar.
        /// 
        /// Name should be unique within the domain context
        /// </summary>
        string Name { get; }
        string Description { get; }
        string LongDescription { get; }
        /// <summary>
        /// Generate a calendar table
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="defaultDayInfo">This is the default value for missing day info</param>
        /// <returns></returns>
        CalendarDays GenerateCalendarDays(DateTime from, DateTime to, DayInfo defaultDayInfo);
    }
}
