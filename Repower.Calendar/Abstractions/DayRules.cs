using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Repower.Calendar
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
