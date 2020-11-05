using System;

namespace Repower.Calendar
{
    /// <summary>
    /// Represent a single rule with the interpretation policy
    /// </summary>
    public class DayRuleItem
    {
        public DayRulePolicy Policy { get; }
        public IDayRule Rule { get; }

        public DayRuleItem(DayRulePolicy policy, IDayRule rule)
        {
            Policy = policy;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }
    }
}
