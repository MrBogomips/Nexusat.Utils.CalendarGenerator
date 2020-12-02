using System;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator
{
    public class RangeNumberMatcher : NumberMatcher, IRangeNumberMatcher, IEquatable<RangeNumberMatcher>
    {
        public RangeNumberMatcher(int? left, int? right)
        {
            if (left is not null && left.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(left), "Must be a non negative value");
            switch (right)
            {
                case not null when right.Value < 0:
                    throw new ArgumentOutOfRangeException(nameof(right), "Must be a non negative value");
                case not null when right < left:
                    throw new ArgumentOutOfRangeException(nameof(right),
                        $"Must be greater than or equal to '{nameof(left)}'");
                default:
                    Left = left;
                    Right = right;
                    break;
            }
        }

        protected RangeNumberMatcher((int? left, int? right) init) : this(init.left, init.right)
        {
        }

        private static Regex ParseRegex { get; } = new Regex(@"^(?<left>\d+)?(?<dots>\.\.)?(?<right>\d+)?$");

        public bool Equals(RangeNumberMatcher other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Left == other.Left && Right == other.Right;
        }

        public int? Left { get; }
        public int? Right { get; }

        public override bool Match(int value)
        {
            return (!Left.HasValue || Left.Value <= value)
                   &&
                   (!Right.HasValue || Right.Value >= value);
        }

        public bool IsLeftOpenRange => !Left.HasValue;
        public bool IsRightOpenRange => !Right.HasValue;
        public bool IsOpenRange => !Left.HasValue || !Right.HasValue;
        public bool IsLeftRightOpenRange => !Left.HasValue && !Right.HasValue;
        public bool IsClosedRange => Left.HasValue && Right.HasValue;
        public bool IsOneValue => Left.HasValue && Right.HasValue && Left.Value == Right.Value;

        public override string ToString()
        {
            return (LeftNumber: Left, RightNumber: Right) switch
            {
                (null, null) => "*",
                (var fy, null) => $"{fy}..",
                (null, var ly) => $"..{ly}",
                var (fy, ly) => fy == ly ? fy.Value.ToString() : $"{fy}..{ly}"
            };
        }

        public void Deconstruct(out int? left, out int? right) => (left, right) = (Left, Right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RangeNumberMatcher) obj);
        }

        public override int GetHashCode() => HashCode.Combine(Left, Right);

        public static bool operator ==(RangeNumberMatcher left, RangeNumberMatcher right) => Equals(left, right);

        public static bool operator !=(RangeNumberMatcher left, RangeNumberMatcher right) => !Equals(left, right);

        /// <summary>
        ///     Parse a string representing a valid number range expression.
        ///     <example>
        ///         Valid strings are:
        ///         * for any valid number
        ///         120.. for any number greater than or equal to 120
        ///         ..120 for any number less than or equal to 120
        ///         100..200 for any number between 100 and 200
        ///         100 for the exact number 100
        ///     </example>
        /// </summary>
        public static bool TryParse(string value, int? minLeft, int? maxLeft, int? minRight, int? maxRight,
            out int? left, out int? right)
        {
            left = right = default;
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value == "..") return false; // regex oversimplified

            var m = ParseRegex.Match(value);
            if (m.Success)
            {
                left = m.Groups["left"].Success ? int.Parse(m.Groups["left"].Value) : (int?) null;
                if (m.Groups["dots"].Success)
                    right = m.Groups["right"].Success ? int.Parse(m.Groups["right"].Value) : (int?) null;
                else
                    right = left;
            }
            else if (value != "*")
            {
                return false; // regex oversimplified
            }

            if (left.HasValue)
            {
                // range check
                if (minLeft.HasValue && left.Value < minLeft.Value) return false;
                if (maxLeft.HasValue && left.Value > maxLeft.Value) return false;
            }
            else if (minLeft.HasValue)
            {
                // normalize left value
                left = minLeft;
            }

            if (right.HasValue)
            {
                // range check
                if (minRight.HasValue && right.Value < minRight.Value) return false;
                if (maxRight.HasValue && right.Value > maxRight.Value) return false;
            }
            else if (maxRight.HasValue)
            {
                // normalize right value
                right = maxRight;
            }

            return true;
        }

        /// <summary>
        ///     Parse a string representing a valid number range expression.
        ///     <example>
        ///         Valid strings are:
        ///         * or *..* for any valid number
        ///         120.. for any number greater than or equal to 120
        ///         ..120 for any number less than or equal to 120
        ///         100..200 for any number between 100 and 200
        ///         100 for the exact number 100
        ///     </example>
        /// </summary>
        public static bool TryParse(string value, out RangeNumberMatcher rangeNumberMatcher)
        {
            if (!TryParse(value, null, null, null, null, out var left, out var right))
            {
                rangeNumberMatcher = null;
                return false;
            }

            rangeNumberMatcher = new RangeNumberMatcher(left, right);
            return true;
        }
    }
}