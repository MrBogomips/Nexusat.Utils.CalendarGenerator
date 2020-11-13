using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     The policy in evaluating a chain of rules
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public enum DayRulePolicy
    {
        /// <summary>
        ///     Continue the evaluation of the rules. Following rules could override this outcome.
        /// </summary>
        [EnumMember] Fallthrough = 0,

        /// <summary>
        ///     Stop the evaluation of the rules and accept this outcome.
        /// </summary>
        [EnumMember] Accept = 3
    }
}