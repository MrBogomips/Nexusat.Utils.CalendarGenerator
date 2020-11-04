using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a rule based on week days
    /// </summary>
    public class WeekdaysRule : IWorkingDayRule
    {
        internal static readonly string NAME = "Weekdays";
        public string Name => NAME;

        private readonly WeekdaysRuleSettings Settings;

        public WeekdaysRule(WeekdaysRuleSettings settings) =>
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
                dayInfo = new WorkingDayInfo { IsWorkingDay = true, WorkingPeriods = setting.First().WorkingPeriods };
                return true;
            }
        }
    }
}
