using System;
using System.Linq;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a rule based on non working week days
    /// </summary>
    public class WeekdaysNonWorkingRule : IWorkingDayRule
    {
        internal static readonly string NAME = "NonWorkingWeekdays";
        public string Name => NAME;

        private readonly WeekdaysNonWorkingRuleSettings Settings;

        public WeekdaysNonWorkingRule(WeekdaysNonWorkingRuleSettings settings) =>
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

        public IWorkingDayInfo GetWorkingDayInfo(DateTime date)
        {
            IWorkingDayInfo dayInfo;
            TryGetWorkingDayInfo(date, out dayInfo);
            return dayInfo;
        }

        public bool TryGetWorkingDayInfo(DateTime date, out IWorkingDayInfo dayInfo)
        {
            dayInfo = null;
            var setting = Settings.Days.Where((d) => d.DayOfWeek == date.DayOfWeek);
            if (setting == null || !setting.Any())
            {
                return false;
            }
            else
            {
                dayInfo = new WorkingDayInfo { IsWorkingDay = false };
                return true;
            }
        }
    }
}
