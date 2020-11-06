using System.Runtime.Serialization;

namespace Nexusat.Calendar
{
    /// <summary>
    /// The policy in evaluating a chain of rules
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public enum DayRulePolicy
    {
        /// <summary>
        /// Useful to set a default value in advance
        /// </summary>
        [EnumMember]
        Fallthrough = 0,
        /// <summary>
        /// If the rule evaluates to <c>true</c> then the evaluation is considered determined.
        /// </summary>
        [EnumMember]
        AcceptIfTrue = 1,
        /// <summary>
        /// If the rule evaluates to <c>false</c> then the evaluation is considered determined.
        /// </summary>
        [EnumMember]
        AcceptIfFalse = 2,
        /// <summary>
        /// If the rule produce any outcome than this is accepted
        /// </summary>
        [EnumMember]
        AcceptAlways = 3,
        
    }
}
