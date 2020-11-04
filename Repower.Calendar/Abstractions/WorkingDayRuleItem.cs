namespace Repower.Calendar
{
    /// <summary>
    /// Represent a single rule with the interpretation policy
    /// </summary>
    public class WorkingDayRuleItem
    {
        public WorkingDayRulePolicy WorkingDayPolicy { get; }
        public IWorkingDayRule WorkingDayRule { get; }
    }
}
