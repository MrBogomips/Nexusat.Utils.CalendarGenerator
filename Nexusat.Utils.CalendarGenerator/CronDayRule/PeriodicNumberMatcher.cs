using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Number matcher expressing a period condition on the value and subsequent steps
    /// </summary>
    public class PeriodicNumberMatcher : RangeNumberMatcher, IEquatable<PeriodicNumberMatcher>
    {
        public int Period { get; }
        
        public PeriodicNumberMatcher(int left, int? right, int period): base(left, right)
        {
            if (period < 2) throw new ArgumentException($"'{nameof(period)}' must be greater than 1");
            if (IsOneValue) throw new ArgumentException("You must specify range");
            Period = period;
        }

        public override bool Match(int value) => base.Match(value) && (value - Left.Value) % Period == 0;

        public override string ToString() => $"{base.ToString()}/{Period}";

        public bool Equals(PeriodicNumberMatcher other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Period == other.Period;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeriodicNumberMatcher) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Period);
        }

        public static bool operator ==(PeriodicNumberMatcher left, PeriodicNumberMatcher right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PeriodicNumberMatcher left, PeriodicNumberMatcher right)
        {
            return !Equals(left, right);
        }
    }
}