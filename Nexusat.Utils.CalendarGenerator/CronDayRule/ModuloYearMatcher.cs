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
    }
}