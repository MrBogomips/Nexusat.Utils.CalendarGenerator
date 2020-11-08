﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
// ReSharper disable MemberCanBePrivate.Global

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represents a time period within a day.
    ///     Hour can assume values between 0 and 24 to correctly represent the End of Day: in this case minute must be 0
    ///     (zero).
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public readonly struct Time : IComparable<Time>, IEquatable<Time>
    {
        [field: DataMember] public short Hour { get; }
        [field: DataMember] public short Minute { get; }

        public Time(short hour, short minute)
        {
            if (hour < 0 || hour > 24)
                throw new ArgumentOutOfRangeException(nameof(hour), "Must be between 0 and 24 range");

            if (hour == 24 && minute != 0)
                throw new ArgumentOutOfRangeException(nameof(minute), "A 24 time must have a minute of 0.");
            if (minute < 0 || minute > 59)
                throw new ArgumentOutOfRangeException(nameof(minute), "Must be between 0 and 59 range");
            Hour = hour;
            Minute = minute;
        }

        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        public override string ToString() => $"{Hour:D2}:{Minute:D2}";

        public readonly int CompareTo(Time other)
        {
            if (Hour > other.Hour) return 1;

            if (Hour < other.Hour) return -1;
            // ASSERT other.Hour == Hour
            if (Minute > other.Minute) return 1;

            if (Minute < other.Minute) return -1;
            return 0;
        }

        public static bool operator <(Time lhs, Time rhs) => lhs.Hour < rhs.Hour || lhs.Hour == rhs.Hour && lhs.Minute < rhs.Minute;

        public static bool operator >(Time lhs, Time rhs) => lhs.Hour > rhs.Hour || lhs.Hour == rhs.Hour && lhs.Minute > rhs.Minute;

        public static bool operator <=(Time lhs, Time rhs) => lhs.Hour <= rhs.Hour && lhs.Minute <= rhs.Minute;

        public static bool operator >=(Time lhs, Time rhs) => lhs.Hour >= rhs.Hour && lhs.Minute >= rhs.Minute;

        public static bool operator ==(Time lhs, Time rhs) => (lhs.Hour, lhs.Minute) == (rhs.Hour, rhs.Minute);

        public static bool operator !=(Time lhs, Time rhs) => (lhs.Hour, lhs.Minute) != (rhs.Hour, rhs.Minute);

        public bool Equals(Time obj) => (Hour, Minute) == (obj.Hour, obj.Minute);

        public override bool Equals(object obj) => obj is Time t && Equals(t);

        /// <summary>
        ///     Returns a serial of the time.
        /// </summary>
        /// <returns></returns>
        public readonly int GetSerial() => Hour * 60 + Minute;

        public override int GetHashCode() => GetSerial();
    }
}