using System;

namespace Nexusat.Utils.CalendarGenerator
{
    internal static class DayOfMonthMatcherHelper
    {
        public static (int?, int?) RangeCtorHelper(int? left, int? right)
        {
            // Validation
            if (left.HasValue && (left.Value < 1 || left.Value > 31))
                throw new ArgumentOutOfRangeException(nameof(left), left.Value, "Must be between 1 and 31");
            if (right.HasValue && (right.Value < 1 || right.Value > 31))
                throw new ArgumentOutOfRangeException(nameof(right), right.Value, "Must be between 1 and 31");
            // Normalization
            if (left == 31) right = 31;
            if (right == 1) left = 1;

            return (left, right);
        }

        public static (int?, int?, int) ModuloCtorHelper(int? left, int? right, int modulo)
        {
            var (varLeft, varRight) = RangeCtorHelper(left, right);
            return (varLeft, varRight, modulo);
        }

        public static (int, int?, int) PeriodicCtorHelper(int left, int? right, int period)
        {
            var (varLeft, varRight) = RangeCtorHelper(left, right);
            return (varLeft.Value, varRight, period);
        }
    }
}