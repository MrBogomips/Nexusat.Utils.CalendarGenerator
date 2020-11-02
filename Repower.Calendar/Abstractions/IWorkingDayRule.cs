using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repower.Calendar
{

    public interface IWorkingDayRule
    {
        /// <summary>
        /// The public name of the rule
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Check if the day is a working day
        /// </summary>
        /// <param name="date">A <see cref="DateTime"/> representing a day</param>
        /// <returns><c>true</c> if the day is a working day</returns>
        bool IsWorkingDay(DateTime date);
        /// <summary>
        /// Retrieve the working periods for a specific day
        /// </summary>
        /// <param name="date">The day of interest</param>
        /// <returns>The list of working periods for that day</returns>
        IList<WorkingDayPeriod> GetWorkingPeriods(DateTime date);
        /// <summary>
        /// Returns a description for the day
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        string GetDayDescription(CultureInfo cultureInfo);
        string GetDayDescription() => GetDayDescription(CultureInfo)
    }
}
