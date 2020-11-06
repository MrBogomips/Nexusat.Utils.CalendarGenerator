using System.Collections.Generic;
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Calendar
{
    /// <summary>
    /// A chain of <see cref="DayRule"/>s to manage the working days
    /// </summary>
    //[DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayRules: List<DayRuleItem>
    {
        public void Add(DayRulePolicy policy, DayRule rule) =>
            Add(new DayRuleItem(policy, rule));
    }
}
