using System.Collections.Generic;

namespace Nexusat.Utils.CalendarGenerator
{
    public interface IDayInfo
    {
        /// <summary>
        /// Represents a working day
        /// </summary>
        bool IsWorkingDay { get; }
        /// <summary>
        /// A short description of the day
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Working periods within the day
        /// </summary>
        IEnumerable<TimePeriod> WorkingPeriods { get; }
    }
}
