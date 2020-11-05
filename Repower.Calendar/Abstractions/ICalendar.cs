using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
