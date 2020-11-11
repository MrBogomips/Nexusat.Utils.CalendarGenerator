namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class MonthMatcherParser : DateMatcherParserBase<IMonthMatcher>
    {
        private MonthMatcherParser()
        {
        }
        
        public static MonthMatcherParser Instance { get; } = new MonthMatcherParser();

        /// <summary>
        ///     Parse a month match expression.
        ///     <example>
        ///         Valid expression are:
        ///         * => Every month
        ///         11 => Only for november
        ///         ..11 => Every month until november
        ///         2.. => Every month starting from february
        ///         2..11 => Every month between february and november
        ///         1..7/2 => Starting from january every 2 years until july
        ///         3../2 => Starting from march every 2 months
        ///         1..7%2 => Every month which reminder modulo 2 is zero between january and july
        ///         2..%3 => Starting from february every month which reminder modulo 3 is zero
        ///     </example>
        /// </summary>
        public override bool TryParse(string value, out IMonthMatcher monthMatcher)
        {
            if (RangeMonthMatcher.TryParse(value, out var rangeMonthMatcher))
            {
                monthMatcher = rangeMonthMatcher;
                return true;
            }

            if (ModuloMonthMatcher.TryParse(value, out var moduloMonthMatcher))
            {
                monthMatcher = moduloMonthMatcher;
                return true;
            }
            if (PeriodicMonthMatcher.TryParse(value, out var periodicMonthMatcher))
            {
                monthMatcher = periodicMonthMatcher;
                return true;
            }

            monthMatcher = default;
            return false;
        }
    }
}