using System;
using System.Collections.Generic;
using System.Text;

namespace Repower.Calendar
{
    /// <summary>
    /// A chain of <see cref="IDayRule"/>s to manage the working days
    /// </summary>
    public class DayRules: List<DayRuleItem>
    {
        public void Add(DayRulePolicy policy, IDayRule rule) =>
            this.Add(new DayRuleItem(policy, rule));
    }
}
