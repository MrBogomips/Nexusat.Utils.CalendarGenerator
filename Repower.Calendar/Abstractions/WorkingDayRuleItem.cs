namespace Repower.Calendar
{
    public class CalendarRuleChainItem
    {
        public CalendarWorkingDayRulePolicy WorkingDayPolicy { get; }
        public IWorkingDayRule WorkingDayRule { get; }
    }
}
