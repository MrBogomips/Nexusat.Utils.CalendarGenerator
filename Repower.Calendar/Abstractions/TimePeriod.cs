using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Repower.Calendar
{
    public readonly struct TimePeriod : IComparable<TimePeriod>
    {
        public Time Begin { get; }
        public Time End { get; }

        public TimePeriod(Time begin ,Time end)
        {
            if (begin > end) throw new ArgumentException("Begin time must precede end time");
            Begin = begin;
            End = end;
        }
        public int CompareTo(TimePeriod other) => Begin.CompareTo(other.Begin);
        

    }
}
