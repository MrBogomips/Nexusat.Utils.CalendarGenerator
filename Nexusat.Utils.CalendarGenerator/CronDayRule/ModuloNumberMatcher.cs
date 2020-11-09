using System;

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
    }
}