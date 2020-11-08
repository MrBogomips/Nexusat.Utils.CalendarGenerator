using System;
using System.Collections.Generic;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     A chain of <see cref="DayRule" />s to manage the working days
    /// </summary>
    //[DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class DayRules : List<DayRuleItem>
    {
        public void Add(DayRulePolicy policy, DayRule rule)
        {
            Add(new DayRuleItem(policy, rule));
        }

        /// <summary>
        ///     Add the rules from the calendar to the collection of rules
        /// </summary>
        /// <param name="calendar"></param>
        public void Add(Calendar calendar)
        {
            if (calendar == null) throw new ArgumentNullException(nameof(calendar));
            AddRange(calendar.DayRules);
        }
    }
}