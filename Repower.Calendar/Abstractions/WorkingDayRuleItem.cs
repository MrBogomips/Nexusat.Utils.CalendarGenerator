using System;

namespace Repower.Calendar
{
    /// <summary>
    /// Represent a single rule with the interpretation policy
    /// </summary>
    public class WorkingDayRuleItem
    {
        public WorkingDayRulePolicy Policy { get; }
        public IWorkingDayRule Rule { get; }

        public WorkingDayRuleItem(WorkingDayRulePolicy policy, IWorkingDayRule rule)
        {
            Policy = policy;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }
    }
}
