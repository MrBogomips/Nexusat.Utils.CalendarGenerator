using System.Collections.Generic;

namespace Nexusat.Calendar
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
        IList<TimePeriod> WorkingPeriods { get; }
    }
}
