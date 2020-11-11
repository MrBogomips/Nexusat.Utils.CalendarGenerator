namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class DayOfMonthMonthMatcherParser : DateMatcherParserBase<IDayOfMonthMatcher>
    {
        private DayOfMonthMonthMatcherParser()
        {
        }
        
        public static DayOfMonthMonthMatcherParser Instance { get; } = new DayOfMonthMonthMatcherParser();

        /// <summary>
        ///     Parse a day of month match expression.
        ///     <example>
        ///         Valid expression are:
        ///         * => Every month
        ///         11 => Only the day 11
        ///         ..11 => Every day from 1 to 11
        ///         2.. => Every day from 2 to 31
        ///         2..11 => Every day between 2 and 11
        ///         1..7/2 => Starting from day 1 every 2 days until the 7th
        ///         3../2 => Starting from day 3 every 2 days till the end of the month
        ///         1..7%2 => Every day which reminder modulo 2 is zero between the 1st and 7th
        ///         2..%3 => Starting from the 2nd every day which reminder modulo 3 is zero
        ///     </example>
        /// </summary>
        public override bool TryParse(string value, out IDayOfMonthMatcher dayOfMonthMatcher)
        {
            if (RangeDayOfMonthMatcher.TryParse(value, out var rangeDayOfMonthMatcher))
            {
                dayOfMonthMatcher = rangeDayOfMonthMatcher;
                return true;
            }

            if (ModuloDayOfMonthMatcher.TryParse(value, out var moduloDayOfMonthMatcher))
            {
                dayOfMonthMatcher = moduloDayOfMonthMatcher;
                return true;
            }
            if (PeriodicDayOfMonthMatcher.TryParse(value, out var periodicDayOfMonthMatcher))
            {
                dayOfMonthMatcher = periodicDayOfMonthMatcher;
                return true;
            }

            dayOfMonthMatcher = default;
            return false;
        }
    }
}