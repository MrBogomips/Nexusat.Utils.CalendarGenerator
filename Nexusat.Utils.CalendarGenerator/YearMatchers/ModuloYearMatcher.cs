using System;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Year matcher expressing a modulo condition on the year value
    /// </summary>
    public class ModuloYearMatcher : ModuloNumberMatcher, IYearMatcher
    {
        public ModuloYearMatcher(int? left, int? right, int modulo) : base(left, right, modulo)
        {
        }

        public bool Match(DateTime date)
        {
            return Match(date.Year);
        }

        public bool IsOneYear => IsOneValue;

        public static bool TryParse(string value, out ModuloYearMatcher moduloYearMatcher)
        {
            if (!TryParse(value, null, null, null, null, out var left, out var right, out var modulo))
            {
                moduloYearMatcher = null;
                return false;
            }

            moduloYearMatcher = new ModuloYearMatcher(left, right, modulo);
            return true;
        }
    }
}