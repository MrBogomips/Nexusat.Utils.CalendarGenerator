using System.Collections.Generic;

namespace Repower.Calendar
{
    public interface IDayInfo
    {
        /// <summary>
        /// Represetns a working day
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
