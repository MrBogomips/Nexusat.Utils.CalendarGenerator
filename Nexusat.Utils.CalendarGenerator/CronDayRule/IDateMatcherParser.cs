using System.Collections.Generic;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IDateMatcherParser<out T> where T:IDateMatcher
    {
        T Parse(string value);
        IEnumerable<T> ParseMulti(string values, string separator);
        /// <summary>
        /// Parse a comma separated list of values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        IEnumerable<T> ParseMulti(string values);
    }
}