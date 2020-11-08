using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public sealed class DayInfo
    {
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public DayInfo(string description = null, IEnumerable<TimePeriod> workingPeriods = null)
        {
            Description = description;
            WorkingPeriods = NormalizeWorkingPeriods(workingPeriods);
            IsWorkingDay = WorkingPeriods is not null;
        }

        /// <summary>
        /// Normalize working periods by sorting them and checking they don't overlap
        /// </summary>
        /// <param name="workingPeriods"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<TimePeriod> NormalizeWorkingPeriods(IEnumerable<TimePeriod> workingPeriods)
        {
            if (workingPeriods == null || !workingPeriods.Any()) return null;

            var retVal = new List<TimePeriod>(workingPeriods);
            retVal.Sort();
            var prevTimePeriod = retVal.First();
            for (var i = 1; i < retVal.Count; i++)
                if (prevTimePeriod.Overlaps(retVal[i]))
                    throw new ArgumentException("Working Periods can't overlap");
            return retVal;
        }
        
        /// <summary>
        /// Parse a day info declaration
        /// </summary>
        /// <param name="description"></param>
        /// <param name="workingPeriods">Represents a valid string for time periods. <see cref="TimePeriod.ParseMulti"/></param>
        /// <returns></returns>
        public DayInfo Parse(string description = null, string workingPeriods = null) =>
            new DayInfo(description, TimePeriod.ParseMulti(workingPeriods));

        /// <summary>
        ///     Represents a working day
        /// </summary>
        [field: DataMember] public bool IsWorkingDay { get; }
        /// <summary>
        ///     A short description of the day
        /// </summary>
        [field: DataMember] public string Description { get; }
        /// <summary>
        ///     Working periods within the day
        /// </summary>
        [field: DataMember] public IEnumerable<TimePeriod> WorkingPeriods { get; }
    }
}