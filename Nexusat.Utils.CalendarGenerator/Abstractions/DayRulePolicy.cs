namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     The policy in evaluating a chain of rules
    /// </summary>
    public enum DayRulePolicy
    {
        /// <summary>
        ///     Continue the evaluation of the rules. Following rules could override this outcome.
        /// </summary>
        Fallthrough = 0,

        /// <summary>
        ///     Stop the evaluation of the rules and accept this outcome.
        /// </summary>
        Accept = 1
    }
}