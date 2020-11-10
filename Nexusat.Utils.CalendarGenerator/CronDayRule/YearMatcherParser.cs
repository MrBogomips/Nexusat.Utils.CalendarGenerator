namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class YearMatcherParser : DateMatcherParserBase<IYearMatcher>
    {
        private YearMatcherParser()
        {
        }
        
        public static YearMatcherParser Instance { get; } = new YearMatcherParser();

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
        public override bool TryParse(string value, out IYearMatcher yearMatcher)
        {
            if (RangeYearMatcher.TryParse(value, out var rangeYearMatcher))
            {
                yearMatcher = rangeYearMatcher;
                return true;
            }

            if (PeriodicYearMatcher.TryParse(value, out var periodYearMatcher))
            {
                yearMatcher = periodYearMatcher;
                return true;
            }

            if (ModuloYearMatcher.TryParse(value, out var moduloYearMatcher))
            {
                yearMatcher = moduloYearMatcher;
                return true;
            }

            if (LeapYearMatcher.TryParse(value, out var leapYearMatcher))
            {
                yearMatcher = leapYearMatcher;
                return true;
            }

            if (NotLeapYearMatcher.TryParse(value, out var notLeapYearMatcher))
            {
                yearMatcher = notLeapYearMatcher;
                return true;
            }

            yearMatcher = default;
            return false;
        }
    }
}