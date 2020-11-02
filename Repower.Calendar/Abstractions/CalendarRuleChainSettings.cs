using System;
using System.Collections.Generic;
using System.Text;

namespace Repower.Calendar
{
    /// <summary>
    /// The policy in evaluating a chain of rules
    /// </summary>
    public enum CalendarWorkingDayRulePolicy
    {
        /// <summary>
        /// If the rule evaluates to <c>true</c> then the evaluation is considered determined.
        /// </summary>
        AcceptIfTrue,
        /// If the rule evaluates to <c>false</c> then the evaluation is considered determined.
        AcceptIfFalse
    }

    public class CalendarRuleChainItem
    {
        public CalendarWorkingDayRulePolicy WorkingDayPolicy { get; }
        public IWorkingDayRule WorkingDayRule { get; }
    }

    class CalendarRuleChainSettings
    {

    }
}
