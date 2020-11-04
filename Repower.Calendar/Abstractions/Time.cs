﻿using System;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a time period within a day.
    /// Hour can assume values between 0 and 24 meanind the End of Day.
    /// Minute can asssume values between 0 and 60 meaninbg the End of Hour.
    /// </summary>
    public readonly struct Time: IComparable<Time>
    {
        public short Hour { get; }
        public short Minute { get; }
        public Time(short hour, short minute)
        {
            if (hour < 0 || hour > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(Hour), "Must be between 0 and 24 range");
            }
            if (minute < 0 || minute > 60)
            {
                throw new ArgumentOutOfRangeException(nameof(Minute), "Must be between 0 and 60 range");
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

        public override int GetHashCode() => Hour ^ Minute;
    }
}