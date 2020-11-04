using System;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a time period within a day.
    /// Hour can assume values between 0 and 24 to correctly represent the End of Day: in this case minute must be 0 (zero).
    /// </summary>
    public readonly struct Time: IComparable<Time>
    {
        public short Hour { get; }
        public short Minute { get; }
        public Time(short hour, short minute)
        {
            if (hour < 0 || hour > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), "Must be between 0 and 24 range");
            } 
            else if (hour == 24 && minute != 0)
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
        public override string ToString() => $"{Hour:00}:{Minute:00}";

        public int CompareTo(Time other)
        {
            if (Hour > other.Hour)
            {
                return 1;
            }
            else if (Hour < other.Hour)
            {
                return -1;
            }
            // ASSERT other.Hour == Hour
            if (Minute > other.Minute)
            {
                return 1;
            } 
            else if (Minute < other.Minute)
            {
                return -1;
            }
            return 0;
        }

        public static bool operator <(Time lhs, Time rhs) =>
            lhs.Hour < rhs.Hour || (lhs.Hour == rhs.Hour && lhs.Minute < rhs.Minute); 
        
        public static bool operator >(Time lhs, Time rhs) =>
            lhs.Hour > rhs.Hour || (lhs.Hour == rhs.Hour && lhs.Minute > rhs.Minute);
        public static bool operator <=(Time lhs, Time rhs) =>
            lhs.Hour <= rhs.Hour && lhs.Minute <= rhs.Minute;
        
        public static bool operator >=(Time lhs, Time rhs) =>
            lhs.Hour >= rhs.Hour && lhs.Minute >= rhs.Minute;
        public static bool operator ==(Time lhs, Time rhs) => 
            lhs.Hour == rhs.Hour && lhs.Minute == rhs.Minute;
        
        public static bool operator !=(Time lhs, Time rhs) => 
            lhs.Hour != rhs.Hour || lhs.Minute != rhs.Minute;

        public bool Equals(Time obj) => this == obj;

        public override bool Equals(object obj)
        {
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
        public int GetSerial() => Hour * 60 + Minute;
        public override int GetHashCode() => GetSerial();
    }
}
