using System;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public class YearMatcherParser: DateMatcherParserBase<IYearMatcher> {
        /// <summary>
        /// Parse a year match expression.
        /// <example>
        /// Valid expression are:
        /// * => Every year
        /// *..* => Every year also
        /// 2020 => Exactly that year
        /// *..2030 => Every year until 2030 comprised
        /// 2020..* => Every year starting from 2020
        /// 2020-2030 => Every year between 2020 and 2030
        /// */Leap => Every leap year
        /// 2020-2030/Leap => Every leap year between 2020 and 2030
        /// */NonLeap => Every non leap year
        /// 2020..2030/2 => Starting from 2020 every 2 years until 2030
        /// 2020/2 => Starting from 2020 every 2 years
        /// 2020/NonLeap => Starting from 2020 every NonLeap year
        /// 2020..2030%2 => Every year which reminder modulo 2 is zero between 2020 and 2030
        /// 2020%3 => Starting from 2020 every year which reminder modulo 3 is zero
        /// </example>
        /// </summary>
        public override bool TryParse(string value, out IYearMatcher yearMatcher)
        {
            throw new NotImplementedException();
        }
    }
}