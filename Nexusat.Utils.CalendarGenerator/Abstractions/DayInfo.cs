using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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

        private static Regex ParseRegex { get; } = new Regex(@"^\[\[((?<description>.*))\]\]\s*(?<timePeriods>(.*))$");

        /// <summary>
        ///     Represents a working day
        /// </summary>
        [field: DataMember(Name = nameof(IsWorkingDay))]
        public bool IsWorkingDay { get; }

        /// <summary>
        ///     A short description of the day
        /// </summary>
        [field: DataMember(Name = nameof(Description))]
        public string Description { get; }

        /// <summary>
        ///     Working periods within the day
        /// </summary>
        [field: DataMember(Name = nameof(WorkingPeriods))]
        public IEnumerable<TimePeriod> WorkingPeriods { get; }

        /// <summary>
        ///     Normalize working periods by sorting them and checking they don't overlap
        /// </summary>
        /// <param name="workingPeriods"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IReadOnlyList<TimePeriod> NormalizeWorkingPeriods(IEnumerable<TimePeriod> workingPeriods)
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
        ///     Parse a day info declaration.
        ///     <example>
        ///         Valid strings are:
        ///         [[]] is a day info without a description and a working time
        ///         [[day description]] is a day info with a description and without a working time
        ///         [[day description]] 00:00-01:00,22:00-23:00 is a day info with a description and a working time
        ///         [[]] 00:00-01:00 is a day info without a description and a working time
        ///     </example>
        /// </summary>
        /// <returns></returns>
        public static bool TryParse(string value, out DayInfo dayInfo)
        {
            dayInfo = default;
            IEnumerable<TimePeriod> timePeriods = default;

            if (string.IsNullOrEmpty(value)) return false;
            var m = ParseRegex.Match(value);
            if (!m.Success) return false;

            var description = m.Groups["description"].Value;
            var strTimePeriods = m.Groups["timePeriods"].Value;
            if (!string.IsNullOrWhiteSpace(strTimePeriods)
                && !TimePeriod.TryParseMulti(strTimePeriods, ",", out timePeriods)) return false;

            if (string.IsNullOrWhiteSpace(description)) description = null;

            dayInfo = new DayInfo(description, timePeriods);

            return true;
        }
    }
}