namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class MonthMatcherParser : DateMatcherParserBase<IMonthMatcher>
    {
        private MonthMatcherParser()
        {
        }
        
        public static MonthMatcherParser Instance { get; } = new MonthMatcherParser();

        /// <summary>
        ///     Parse a year match expression.
        ///     <example>
        ///         Valid expression are:
        ///         * => Every year
        ///         2020 => Exactly that year
        ///         ..2030 => Every year until 2030 comprised
        ///         2020.. => Every year starting from 2020
        ///         2020..2030 => Every year between 2020 and 2030
        ///         */Leap => Every leap year
        ///         2020..2030/Leap => Every leap year between 2020 and 2030
        ///         */NotLeap => Every non leap year
        ///         2020..2030/2 => Starting from 2020 every 2 years until 2030
        ///         2020../2 => Starting from 2020 every 2 years
        ///         2020../NotLeap => Starting from 2020 every NonLeap year
        ///         2020..2030%2 => Every year which reminder modulo 2 is zero between 2020 and 2030
        ///         2020..%3 => Starting from 2020 every year which reminder modulo 3 is zero
        ///     </example>
        /// </summary>
        public override bool TryParse(string value, out IMonthMatcher monthMatcher)
        {
            if (RangeMonthMatcher.TryParse(value, out var rangeMonthMatcher))
            {
                monthMatcher = rangeMonthMatcher;
                return true;
            }

            // TODO: 

            monthMatcher = default;
            return false;
        }
    }
}