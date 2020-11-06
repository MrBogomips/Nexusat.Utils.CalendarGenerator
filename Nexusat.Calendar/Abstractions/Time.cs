using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Nexusat.Calendar
{
    /// <summary>
    /// Represents a time period within a day.
    /// Hour can assume values between 0 and 24 to correctly represent the End of Day: in this case minute must be 0 (zero).
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public struct Time: IComparable<Time>
    {
        [DataMember]
        public short Hour { get; private set; }
        [DataMember]
        public short Minute { get; private set; }
        public Time(short hour, short minute)
        {
            if (hour < 0 || hour > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), "Must be between 0 and 24 range");
            }

            if (hour == 24 && minute != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), "A 24 time must have a minute of 0.");
            }
            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), "Must be between 0 and 59 range");
            }
            Hour = hour;
            Minute = minute;
        }
        
        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        public override string ToString() => $"{Hour:D2}:{Minute:D2}";

        public readonly int CompareTo(Time other)
        {
            if (Hour > other.Hour)
            {
                return 1;
            }

            if (Hour < other.Hour)
            {
                return -1;
            }
            // ASSERT other.Hour == Hour
            if (Minute > other.Minute)
            {
                return 1;
            }

            if (Minute < other.Minute)
            {
                return -1;
            }
            return 0;
        }

        public static bool operator <(Time lhs, Time rhs) =>
            lhs.Hour < rhs.Hour || lhs.Hour == rhs.Hour && lhs.Minute < rhs.Minute; 
        
        public static bool operator >(Time lhs, Time rhs) =>
            lhs.Hour > rhs.Hour || lhs.Hour == rhs.Hour && lhs.Minute > rhs.Minute;
        public static bool operator <=(Time lhs, Time rhs) =>
            lhs.Hour <= rhs.Hour && lhs.Minute <= rhs.Minute;
        
        public static bool operator >=(Time lhs, Time rhs) =>
            lhs.Hour >= rhs.Hour && lhs.Minute >= rhs.Minute;
        public static bool operator ==(Time lhs, Time rhs) => 
            lhs.Hour == rhs.Hour && lhs.Minute == rhs.Minute;
        
        public static bool operator !=(Time lhs, Time rhs) => 
            lhs.Hour != rhs.Hour || lhs.Minute != rhs.Minute;

        private bool Equals(Time obj) => this == obj;

        public override bool Equals(object obj)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Time)obj);
        }
        /// <summary>
        /// Returns a serial of the time.
        /// </summary>
        /// <returns></returns>
        public readonly int GetSerial() => Hour * 60 + Minute;
        public override int GetHashCode() => GetSerial();
    }
}
