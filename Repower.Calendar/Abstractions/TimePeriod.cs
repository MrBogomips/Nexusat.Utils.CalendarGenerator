using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a contigous, right opened, period within a day.
    /// A whole day is represented by the period [00:00, 24:00[, as ISO 8601 recommendes.
    /// .Net Class Library, unfortunately, isn't compliant with that time period representation.
    /// </summary>
    [Serializable]
    public readonly struct TimePeriod : IComparable<TimePeriod>
    {
        public static readonly TimePeriod AllDay = new TimePeriod(new Time(), new Time(24, 0));
        public Time Begin { get; }
        public Time End { get; }

        public TimePeriod(Time begin, Time end)
        {
            if (begin > end) throw new ArgumentException("Begin time must precede end time");
            Begin = begin;
            End = end;
        }
        public TimePeriod(short beginHour, short beginMinute, short endHour, short endMinute) : 
            this(new Time(beginHour, beginMinute), new Time(endHour, endMinute)) { }
        public int CompareTo(TimePeriod other) => Begin.CompareTo(other.Begin);

        /// <summary>
        /// Check if two timeperiods overlaps
        /// </summary>
        /// <param name="other"></param>
        /// <returns><c>true</c> in case of overlapping</returns>
        public bool Overlaps(TimePeriod other) {
            var this_begin = Begin.GetSerial();
            var this_end = End.GetSerial();
            var other_begin = other.Begin.GetSerial();
            var other_end = other.End.GetSerial();

            return (other_begin < this_end && other_end > this_begin) ||
                (other_end > this_begin && other_end < this_end);
        }

        /// <summary>
        /// Returns the number of minutes covered in the period.
        /// </summary>
        /// <returns>0 in the case of a period where Begin and End are the same</returns>
        public int TotalMinutes => End.GetSerial() - Begin.GetSerial();


        public static implicit operator TimeSpan(TimePeriod timePeriod) =>
            new TimeSpan(
                hours: timePeriod.End.Hour - timePeriod.Begin.Hour, 
                minutes: timePeriod.End.Minute - timePeriod.Begin.Minute,
                seconds: 0);
        
    }
}
