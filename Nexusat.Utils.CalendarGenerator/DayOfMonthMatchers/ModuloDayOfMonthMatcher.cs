using System;
using static Nexusat.Utils.CalendarGenerator.DayOfMonthMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Day of month matcher expressing a modulo condition on the day value
    /// </summary>
    public class ModuloDayOfMonthMatcher : ModuloNumberMatcher, IDayOfMonthMatcher
    {
        public ModuloDayOfMonthMatcher(int? left, int? right, int modulo) : base(ModuloCtorHelper(left, right, modulo)) { }

        public bool Match(DateTime date) => Match(date.Day);

        public bool IsOneDay => IsOneValue;

        public static bool TryParse(string value, out ModuloDayOfMonthMatcher moduloMonthMatcher)
        {
            moduloMonthMatcher = default;
            if (!TryParse(value, 1, 31, 1, 31, out var left, out var right, out var modulo)) return false;

            moduloMonthMatcher = new ModuloDayOfMonthMatcher(left, right, modulo);
            return true;
        }

        public override string ToString() => Left == 1 && Right == 31 ? $"*%{Modulo}" : base.ToString();
    }
}