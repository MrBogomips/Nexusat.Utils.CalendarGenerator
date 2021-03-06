using System;

namespace Nexusat.Utils.CalendarGenerator
{
    internal static class MonthMatcherHelper
    {
        public static (int?, int?) RangeCtorHelper(int? left, int? right)
        {
            // Validation
            if (left.HasValue && (left.Value < 1 || left.Value > 12))
                throw new ArgumentOutOfRangeException(nameof(left), left.Value, "Must be between 1 and 12");
            if (right.HasValue && (right.Value < 1 || right.Value > 12))
                throw new ArgumentOutOfRangeException(nameof(right), right.Value, "Must be between 1 and 12");
            // Normalization
            if (left == 12) right = 12;
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

    internal static class DayOfWeekMatcherHelper
    {
        public static (int?, int?) RangeCtorHelper(int? left, int? right)
        {
            // Validation
            if (left.HasValue && (left.Value < 0 || left.Value > 7))
                throw new ArgumentOutOfRangeException(nameof(left), left.Value, "Must be between 0 and 7");
            if (right.HasValue && (right.Value < 0 || right.Value > 7))
                throw new ArgumentOutOfRangeException(nameof(right), right.Value, "Must be between 0 and 7");
            // Normalization
            if (left == 7) right = 7;
            if (right == 0) left = 0;

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