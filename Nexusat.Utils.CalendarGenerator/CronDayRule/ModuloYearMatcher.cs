using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    /// <summary>
    /// Year matcher expressing a modulo condition on the year value
    /// </summary>
    public class ModuloYearMatcher : ModuloNumberMatcher, IYearMatcher
    {
        public ModuloYearMatcher(int? left, int? right, int modulo) : base(left, right, modulo)
        {
        }

        public bool Match(DateTime date) => Match(date.Year);

        public bool IsOneYear => IsOneValue;
        
        public static bool TryParse(string value, out ModuloYearMatcher rangeNumberMatcher)
        {
            if (!TryParse(value, out var left, out var right, out var modulo))
            {
                rangeNumberMatcher = null;
                return false;
            }

            rangeNumberMatcher = new ModuloYearMatcher(left, right, modulo);
            return true;
        }
    }
}