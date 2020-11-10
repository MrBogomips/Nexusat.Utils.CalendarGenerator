using System;
using System.Text.RegularExpressions;

// ReSharper disable PossibleInvalidOperationException

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Number matcher expressing a period condition on the value and subsequent steps
    /// </summary>
    public class PeriodicNumberMatcher : RangeNumberMatcher, IEquatable<PeriodicNumberMatcher>
    {
        public int Period { get; }

        public PeriodicNumberMatcher(int left, int? right, int period) : base(left, right)
        {
            if (period < 2) throw new ArgumentException($"'{nameof(period)}' must be greater than 1");
            if (IsOneValue) throw new ArgumentException("You must specify range");
            Period = period;
        }
        
        public PeriodicNumberMatcher((int left, int? right, int period) init) : this(init.left, init.right, init.period)
        {
            
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
            return obj.GetType() == GetType() && Equals((PeriodicNumberMatcher) obj);
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

        private static Regex ParseRegex { get; } = new Regex(@"^(?<range>.+)/(?<period>\d+)?$");

        public static bool TryParse(string value, int? minLeft, int? maxLeft, int? minRight, int? maxRight, out int left, out int? right, out int period)
        {
            left = default;
            right = default;
            period = default;
            if (string.IsNullOrWhiteSpace(value)) return false;

            var m = ParseRegex.Match(value);
            if (!m.Success) return false;

            var range = m.Groups["range"].Value;
            if (!TryParse(range, minLeft, maxLeft, minRight, maxRight, out var varLeft, out var varRight)) return false;
            if (!varLeft.HasValue) return false; // Periodic matcher requires an initial value
            if (varLeft == varRight) return false; // Periodic matcher can't be defined on a single number
            period = int.Parse(m.Groups["period"].Value);
            if (period < 2) return false; // Period must be at least 2
            left = varLeft.Value;
            right = varRight;

            return true;
        }

        public static bool TryParse(string value, out PeriodicNumberMatcher periodicNumberMatcher)
        {
            periodicNumberMatcher = default;
            if (!TryParse(value, null, null, null, null, out var left, out var right, out var period)) return false;
            periodicNumberMatcher = new PeriodicNumberMatcher(left, right, period);
            return true;
        }
        
        
    }
}