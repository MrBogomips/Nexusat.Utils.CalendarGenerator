using System;

namespace Repower.Calendar
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
        /// <param name="defaultDayInfo">This is the default value for missing dayinfo</param>
        /// <returns></returns>
        CalendarDays GenerateCalendarDays(DateTime from, DateTime to, DayInfo defaultDayInfo);
    }
}
