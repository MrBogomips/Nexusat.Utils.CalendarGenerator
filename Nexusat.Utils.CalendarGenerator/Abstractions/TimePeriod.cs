using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represents a contiguous, right opened, period within a day.
    ///     A whole day is represented by the period [00:00, 24:00[, as ISO 8601 recommends.
    ///     .Net Class Library, unfortunately, isn't compliant with that time period representation.
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public readonly struct TimePeriod : IComparable<TimePeriod>, IEquatable<TimePeriod>
    {
        public static TimePeriod AllDay { get; } = new TimePeriod(new Time(), new Time(24, 0));
        [field: DataMember] public Time Begin { get; }
        [field: DataMember] public Time End { get; }

        public TimePeriod(Time begin, Time end)
        {
            if (begin > end) throw new ArgumentException("Begin time must precede end time");
            Begin = begin;
            End = end;
        }

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
        public readonly bool Overlaps(TimePeriod other)
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
        public readonly int TotalMinutes => End.GetSerial() - Begin.GetSerial();


        public static implicit operator TimeSpan(TimePeriod timePeriod)
        {
            return new TimeSpan(
                timePeriod.End.Hour - timePeriod.Begin.Hour,
                timePeriod.End.Minute - timePeriod.Begin.Minute,
                0);
        }
        
        private static Regex ParseRegEx { get; } = new Regex(@"^(\d{2}:\d{2})-(\d{2}:\d{2})$");

        /// <summary>
        /// Parse a string in the format "00:00-24:00"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static TimePeriod Parse(string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));

            var match = ParseRegEx.Match(value);
            if (!match.Success) throw new ArgumentException("Time syntax should be 00:00-24:00", nameof(value));
            var begin = Time.Parse(match.Groups[1].Value);
            var end = Time.Parse(match.Groups[2].Value);
            return new TimePeriod(begin, end);
        }
        /// <summary>
        /// Parse a string in the format "00:00-12:00 13:00-18:00 18:00-20"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<TimePeriod> ParseMulti(string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            
            return value.Split(' ').Select(TimePeriod.Parse).ToList();
        }

        public override string ToString() => $"{Begin}-{End}";

        public bool Equals(TimePeriod other) => (Begin, End) == (other.Begin, other.End);
        public override bool Equals(object obj) => obj is TimePeriod other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Begin, End);
        public static bool operator ==(TimePeriod lhs, TimePeriod rhs) => lhs.Equals(rhs);
        public static bool operator !=(TimePeriod lhs, TimePeriod rhs) => !lhs.Equals(rhs);
    }
}