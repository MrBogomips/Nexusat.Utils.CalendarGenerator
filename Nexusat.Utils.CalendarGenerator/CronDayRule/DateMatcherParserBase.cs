using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public abstract class DateMatcherParserBase<T>: IDateMatcherParseMulti<T> where T:IDateMatcher {
        public abstract bool TryParse(string value, out T dateMatcher);
        public virtual bool TryParseMulti(string values, string separator, out IEnumerable<T> dateMatchers)
        {
            if (string.IsNullOrWhiteSpace(values))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(values));
            if (string.IsNullOrEmpty(separator))
                throw new ArgumentException("Value cannot be null or empty.", nameof(separator));
            dateMatchers = default;
            var varDateMatchers = new List<T>();
            foreach (var value in values.Split(separator))
            {
                if (!TryParse(value, out var dateMatcher)) return false;
                varDateMatchers.Add(dateMatcher);
            }

            dateMatchers = varDateMatchers;
            return true;
        }

        public bool TryParseMulti(string values, out IEnumerable<T> dateMatchers) 
            => TryParseMulti(values, ",", out dateMatchers);
    }
}