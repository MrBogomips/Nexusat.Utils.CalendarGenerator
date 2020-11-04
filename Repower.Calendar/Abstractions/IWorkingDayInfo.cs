using System;
using System.Collections.Generic;
using System.Text;

namespace Repower.Calendar
{
    public interface IWorkingDayInfo
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
