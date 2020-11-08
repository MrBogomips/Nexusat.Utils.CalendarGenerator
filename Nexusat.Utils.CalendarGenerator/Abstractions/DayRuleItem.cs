using System;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represent a single rule with the interpretation policy
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar", Name = "DayRule")]
    public class DayRuleItem
    {
        public DayRuleItem(DayRulePolicy policy, DayRule rule)
        {
            Policy = policy;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        [DataMember] public DayRulePolicy Policy { get; private set; }

        [DataMember] public DayRule Rule { get; private set; }
    }
}