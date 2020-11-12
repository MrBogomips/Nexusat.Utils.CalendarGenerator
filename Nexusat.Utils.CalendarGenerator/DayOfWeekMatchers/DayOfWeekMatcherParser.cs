namespace Nexusat.Utils.CalendarGenerator
{
    public class DayOfWeekMatcherParser : DateMatcherParserBase<IDayOfWeekMatcher>
    {
        private DayOfWeekMatcherParser()
        {
        }

        public static DayOfWeekMatcherParser Instance { get; } = new DayOfWeekMatcherParser();

        /// <summary>
        ///     Parse a day of week match expression.
        ///     Sunday can be represented with 0 or 7. This oversimplifies some rules.
        ///     <example>
        ///         Valid expression are:
        ///         * => Every day of week
        ///         0 => Sunday
        ///         7 => Sunday also
        ///         0..5 => From Sunday to Friday
        ///         6..7 => From Saturday to Sunday (i.e. the weekend)
        ///     </example>
        /// </summary>
        public override bool TryParse(string value, out IDayOfWeekMatcher dayOfWeekMatcher)
        {
            if (RangeDayOfWeekMatcher.TryParse(value, out var rangeDayOfMonthMatcher))
            {
                dayOfWeekMatcher = rangeDayOfMonthMatcher;
                return true;
            }

            dayOfWeekMatcher = default;
            return false;
        }
    }
}