using System.Collections.Generic;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IDateMatcherParser<T> where T:IDateMatcher
    {
        bool TryParse(string value, out T dateMatcher);
        bool TryParseMulti(string values, string separator, out IEnumerable<T> dateMatchers);

        /// <summary>
        /// Parse a comma separated list of values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="dateMatchers"></param>
        /// <returns></returns>
        bool TryParseMulti(string values, out IEnumerable<T> dateMatchers);
    }
}