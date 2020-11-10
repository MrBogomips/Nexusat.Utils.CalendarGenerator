using System;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Number matcher expressing a modulo condition on value
    /// </summary>
    public class ModuloNumberMatcher : RangeNumberMatcher, IEquatable<ModuloNumberMatcher>
    {
        public int Modulo { get; }
        public ModuloNumberMatcher(int? left, int? right, int modulo): base(left, right)
        {
            if (modulo < 2) throw new ArgumentException($"'{nameof(modulo)}' must be greater than 1");
            if (IsOneValue) throw new ArgumentException("You must specify a range");
            Modulo = modulo;
        }
        
        public override bool Match(int value) => base.Match(value) && value % Modulo == 0;
        public override string ToString() => $"{base.ToString()}%{Modulo}";

        public bool Equals(ModuloNumberMatcher other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Modulo == other.Modulo;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModuloNumberMatcher) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Modulo);
        }

        public static bool operator ==(ModuloNumberMatcher left, ModuloNumberMatcher right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ModuloNumberMatcher left, ModuloNumberMatcher right)
        {
            return !Equals(left, right);
        }
        
        
        private static Regex ParseRegex { get; } = new Regex(@"^(?<range>.+)%(?<modulo>\d+)?$");
        
        public static bool TryParse(string value, out int? left, out int? right, out int modulo)
        {
            left = default;
            right = default;
            modulo = default;
            if (string.IsNullOrWhiteSpace(value)) return false;
            
            var m = ParseRegex.Match(value);
            if (!m.Success) return false;

            var range = m.Groups["range"].Value;
            if (!RangeNumberMatcher.TryParse(range, out var varLeft, out var varRight)) return false;
            if (varLeft.HasValue && varLeft == varRight) return false; // Modulo matcher can't be defined on a single number
            modulo = int.Parse(m.Groups["modulo"].Value);
            if (modulo < 2) return false; // Modulo must be at least 2
            left = varLeft;
            right = varRight;

            return true;
        }

        public static bool TryParse(string value, out ModuloNumberMatcher moduloNumberMatcher)
        {
            moduloNumberMatcher = default;
            if (!TryParse(value, out var left, out var right, out var modulo)) return false;
            moduloNumberMatcher = new ModuloNumberMatcher(left, right, modulo);
            return true;
        }
    }
}