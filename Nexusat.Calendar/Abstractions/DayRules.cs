using System.Collections.Generic;

namespace Nexusat.Calendar
{
    /// <summary>
    /// A chain of <see cref="DayRule"/>s to manage the working days
    /// </summary>
    //[DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayRules: List<DayRuleItem>
    {
        public void Add(DayRulePolicy policy, DayRule rule) =>
            this.Add(new DayRuleItem(policy, rule));
    }
}
