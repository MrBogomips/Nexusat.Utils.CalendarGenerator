using System;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class RangeNumberMatcher: NumberMatcher, IRangeNumberMatcher, IEquatable<RangeNumberMatcher>
    {
        public int? Left { get; }
        public int? Right { get; }
        public override bool Match(int value) => (!Left.HasValue || Left.Value <= value)
                                                 &&
                                                 (!Right.HasValue || Right.Value >= value);

        public RangeNumberMatcher(int? left, int? right)
        {
            if (left is not null && left.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(left), "Must be a positive value");
            if (right is not null && right.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(right), "Must be a positive value");
            if (right is not null && right < left)
                throw new ArgumentOutOfRangeException(nameof(right), $"Must be greater than or equal to '{nameof(left)}'");
            Left = left;
            Right = right;
        }
        
        public bool IsLeftOpenRange => !Left.HasValue;
        public bool IsRightOpenRange => !Right.HasValue;
        public bool IsOpenRange => !Left.HasValue || !Right.HasValue;
        public bool IsLeftRightOpenRange => !Left.HasValue && !Right.HasValue;
        public bool IsClosedRange => Left.HasValue && Right.HasValue;
        public bool IsOneValue => Left.HasValue && Right.HasValue && Left.Value == Right.Value;

        public override string ToString() => (LeftNumber: Left, RightNumber: Right) switch {
            (null, null) => "*",
            (var fy, null) => $"{fy}..",
            (null, var ly) => $"..{ly}",
            var (fy, ly) => fy == ly ? fy.Value.ToString() : $"{fy}..{ly}"
        };
        
        public void Deconstruct(out int? left, out int? right)
        {
            left = Left;
            right = Right;
        }

        public bool Equals(RangeNumberMatcher other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Left == other.Left && Right == other.Right;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RangeNumberMatcher) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Right);
        }

        public static bool operator ==(RangeNumberMatcher left, RangeNumberMatcher right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RangeNumberMatcher left, RangeNumberMatcher right)
        {
            return !Equals(left, right);
        }
        
        internal static Regex ParseRegex { get; } = new Regex("");

        public static bool TryParse(string value, out RangeNumberMatcher rangeNumberMatcher)
        {
            throw new NotImplementedException();
        }
    }
}