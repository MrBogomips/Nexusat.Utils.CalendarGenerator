using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator
{
    public static class DayRuleParser
    {
        private static Regex ParseRegex { get; } = new Regex(@"^\s*
(?<year>\S+)\s+
(?<month>\S+)\s+
(?<dayOfMonth>\S+)\s+
(?<dayOfWeek>\S+)\s*
(\s+(\[\[(?<description>.*)\]\])?\s*
(?<timePeriods>(\d{2}:\d{2}-\d{2}:\d{2})?(,\d{2}:\d{2}-\d{2}:\d{2})*))?\s*
(?<comments>#.*)?$".Replace("\n", ""));

        /// <summary>
        ///     Parse a day rule expression
        ///     A Day Rule Expression is:
        ///     * * * * [[{description}]] {working periods}  #{comments}
        ///     ^ ^ ^ ^ ^                     ^              ^
        ///     | | | | |                     |              +- (Optional) EoL comment
        ///     | | | | |                     |
        ///     | | | | |                     +---------------- (Optional) Comma separated list of working periods
        ///     | | | | |
        ///     | | | | +-------------------------------------- (Optional) Description of the day
        ///     | | | |
        ///     | | | +---------------------------------------- Day of Week rule
        ///     | | |
        ///     | | +------------------------------------------ Day of Month rule
        ///     | |
        ///     | +-------------------------------------------- Month rule
        ///     |
        ///     +---------------------------------------------- Year rule
        ///     <example>
        ///         Every day non working day: * * * * [[]]
        ///         Every day working day: * * * * [[]] 08:30-17:30
        ///         Vacancy on 29th of february: * * 29 * * [[vacancy on the extra day]]
        ///         Vacancy on 1st of march of leap years: */Leap 3 1 * [[Vacancy on the extra day]]
        ///         Ordinary working days: * * * 1..5 [[]] 08:30-13:30,14:30-17:30
        ///     </example>
        /// </summary>
        public static bool TryParse(string value, out DayRule dayRule)
        {
            dayRule = default;
            if (string.IsNullOrWhiteSpace(value)) return false;
            var m = ParseRegex.Match(value);
            if (!m.Success) return false;

            var year = m.Groups["year"].Value;
            var month = m.Groups["month"].Value;
            var dayOfMonth = m.Groups["dayOfMonth"].Value;
            var dayOfWeek = m.Groups["dayOfWeek"].Value;
            var description = m.Groups["description"].Value;
            var strTimePeriods = m.Groups["timePeriods"].Value;

            if (!YearMatcherParser.Instance.TryParseMulti(year, out var yearMatchers)) return false;
            if (!MonthMatcherParser.Instance.TryParseMulti(month, out var monthMatchers)) return false;
            if (!DayOfMonthMatcherParser.Instance.TryParseMulti(dayOfMonth, out var dayOfMonthMatchers)) return false;
            if (!DayOfWeekMatcherParser.Instance.TryParseMulti(dayOfWeek, out var dayOfWeekMatchers)) return false;

            IEnumerable<TimePeriod> timePeriods = null;
            if (!string.IsNullOrWhiteSpace(strTimePeriods)
                && !TimePeriod.TryParseMulti(strTimePeriods, ",", out timePeriods))
                return false;

            if (string.IsNullOrWhiteSpace(description)) description = null;

            dayRule = new DayRule(description, yearMatchers, monthMatchers, dayOfMonthMatchers, dayOfWeekMatchers,
                timePeriods);

            return true;
        }

        public static DayRule Parse(string value)
        {
            if (!TryParse(value, out var cronDayRule))
                throw new ArgumentException($"'{value}' is an invalid day rule", nameof(value));
            return cronDayRule;
        }
    }
}