using System;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represent a single rule with the interpretation policy
    /// </summary>
    public class CalendarRule
    {
        public CalendarRule(DayRulePolicy policy, DayRule rule)
        {
            Policy = policy;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public DayRulePolicy Policy { get; private set; }

        public DayRule Rule { get; private set; }
    }
}