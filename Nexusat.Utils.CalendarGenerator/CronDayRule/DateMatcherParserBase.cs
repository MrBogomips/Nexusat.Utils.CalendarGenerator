using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public abstract class DateMatcherParserBase<T>: IDateMatcherParser<T> where T:IDateMatcher {
        public abstract T Parse(string value);
        public virtual IEnumerable<T> ParseMulti(string values, string separator)
        {
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            if (string.IsNullOrWhiteSpace(values))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(values));
            return values.Split(separator).Select(Parse);
        }

        public IEnumerable<T> ParseMulti(string values) => ParseMulti(values, ",");
    }
}