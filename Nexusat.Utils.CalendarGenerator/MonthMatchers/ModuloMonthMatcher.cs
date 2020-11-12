using System;
using static Nexusat.Utils.CalendarGenerator.MonthMatcherHelper;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Month matcher expressing a modulo condition on the month value
    /// </summary>
    public class ModuloMonthMatcher : ModuloNumberMatcher, IMonthMatcher
    {
        public ModuloMonthMatcher(int? left, int? right, int modulo) : base(ModuloCtorHelper(left, right, modulo))
        {
        }

        public bool Match(DateTime date)
        {
            return Match(date.Month);
        }

        public bool IsOneMonth => IsOneValue;

        public static bool TryParse(string value, out ModuloMonthMatcher moduloMonthMatcher)
        {
            moduloMonthMatcher = default;
            if (!TryParse(value, 1, 12, 1, 12, out var left, out var right, out var modulo)) return false;

            moduloMonthMatcher = new ModuloMonthMatcher(left, right, modulo);
            return true;
        }

        public override string ToString()
        {
            return Left == 1 && Right == 12 ? $"*%{Modulo}" : base.ToString();
        }
    }
}