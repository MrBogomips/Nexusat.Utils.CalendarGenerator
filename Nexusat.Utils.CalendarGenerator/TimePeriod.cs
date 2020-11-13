using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represents a contiguous, right opened, period within a day.
    ///     A whole day is represented by the period [00:00, 24:00[, as ISO 8601 recommends.
    ///     .Net Class Library, unfortunately, isn't compliant with that time period representation.
    /// </summary>
    public readonly struct TimePeriod : IComparable<TimePeriod>, IEquatable<TimePeriod>
    {
        public static TimePeriod AllDay { get; } = new TimePeriod(new Time(), new Time(24, 0));

        public Time Begin { get; }

        public Time End { get; }

        public TimePeriod(Time begin, Time end)
        {
            if (begin > end) throw new ArgumentException("Begin time must precede end time");
            Begin = begin;
            End = end;
        }

        /// <summary>
        /// </summary>
        /// <param name="beginHour"></param>
        /// <param name="beginMinute"></param>
        /// <param name="endHour"></param>
        /// <param name="endMinute"></param>
        public TimePeriod(short beginHour, short beginMinute, short endHour, short endMinute) :
            this(new Time(beginHour, beginMinute), new Time(endHour, endMinute))
        {
        }

        public int CompareTo(TimePeriod other)
        {
            return Begin.CompareTo(other.Begin);
        }

        /// <summary>
        ///     Check if two time periods overlaps
        /// </summary>
        /// <param name="other"></param>
        /// <returns><c>true</c> in case of overlapping</returns>
        public bool Overlaps(TimePeriod other)
        {
            var thisBegin = Begin.GetSerial();
            var thisEnd = End.GetSerial();
            var otherBegin = other.Begin.GetSerial();
            var otherEnd = other.End.GetSerial();

            return otherBegin < thisEnd && otherEnd > thisBegin ||
                   otherEnd > thisBegin && otherEnd < thisEnd;
        }

        /// <summary>
        ///     Returns the number of minutes covered in the period.
        /// </summary>
        /// <returns>0 in the case of a period where Begin and End are the same</returns>
        public int TotalMinutes => End.GetSerial() - Begin.GetSerial();


        public static implicit operator TimeSpan(TimePeriod timePeriod)
        {
            return new TimeSpan(
                timePeriod.End.Hour - timePeriod.Begin.Hour,
                timePeriod.End.Minute - timePeriod.Begin.Minute,
                0);
        }

        private static Regex ParseRegEx { get; } = new Regex(@"^(\d{2}:\d{2})-(\d{2}:\d{2})$");

        /// <summary>
        ///     Parse a string in the format "00:00-24:00"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out TimePeriod timePeriod)
        {
            timePeriod = default;
            if (value is null) throw new ArgumentNullException(nameof(value));

            var match = ParseRegEx.Match(value);
            if (!match.Success) return false;
            var begin = Time.Parse(match.Groups[1].Value);
            var end = Time.Parse(match.Groups[2].Value);
            timePeriod = new TimePeriod(begin, end);
            return true;
        }

        public static TimePeriod Parse(string value)
        {
            if (!TryParse(value, out var timePeriod))
                throw new ArgumentException($"'{nameof(value)}' should be in the form HH:MM-HH:MM");
            return timePeriod;
        }

        /// <summary>
        ///     Parse a string in the format "00:00-12:00,13:00-18:00,18:00-20"
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <param name="timePeriods"></param>
        /// <returns></returns>
        public static bool TryParseMulti(string values, string separator, out IEnumerable<TimePeriod> timePeriods)
        {
            timePeriods = default;
            if (string.IsNullOrEmpty(separator)) throw new ArgumentException("Provide a separator", nameof(separator));
            if (string.IsNullOrEmpty(values)) return false;

            var varTimePeriods = new List<TimePeriod>();

            foreach (var value in values.Split(separator))
            {
                if (!TryParse(value, out var timePeriod)) return false;
                varTimePeriods.Add(timePeriod);
            }

            timePeriods = varTimePeriods;
            return true;
        }

        public static IEnumerable<TimePeriod> ParseMulti(string value, string separator)
        {
            if (!TryParseMulti(value, separator, out var timePeriods))
                throw new ArgumentException(
                    $"'{nameof(value)}' should be in the form HH:MM-HH:MM{separator}HH:MM-HH:MM{separator}...");
            return timePeriods;
        }

        public override string ToString()
        {
            return $"{Begin}-{End}";
        }

        public bool Equals(TimePeriod other)
        {
            return (Begin, End) == (other.Begin, other.End);
        }

        public override bool Equals(object obj)
        {
            return obj is TimePeriod other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Begin, End);
        }

        public static bool operator ==(TimePeriod lhs, TimePeriod rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TimePeriod lhs, TimePeriod rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}