namespace Repower.Calendar
{
    /// <summary>
    /// The policy in evaluating a chain of rules
    /// </summary>
    public enum WorkingDayRulePolicy
    {
        /// <summary>
        /// Useful to set a default value in advance
        /// </summary>
        Fallthrough = 0,
        /// <summary>
        /// If the rule evaluates to <c>true</c> then the evaluation is considered determined.
        /// </summary>
        AcceptIfTrue = 1,
        /// <summary>
        /// If the rule evaluates to <c>false</c> then the evaluation is considered determined.
        /// </summary>
        AcceptIfFalse = 2,
        /// <summary>
        /// If the rule produce any outcome than this is accepted
        /// </summary>
        AcceptAlways = 3,
        
    }
}
