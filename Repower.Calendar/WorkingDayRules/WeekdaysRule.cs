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

        private readonly WeekdayRuleSettings Settings;

        public WeekdaysRule(WeekdayRuleSettings settings) =>
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

        public IWorkingDayInfo GetWorkingDayInfo(DateTime date)
        {  
            var setting = Settings.Days.Where((d) => d.DayOfWeek == date.DayOfWeek).First();

            if (setting == null) return null;

            return new WorkingDayInfo { IsWorkingDay = true, WorkingPeriods = setting.WorkingPeriods };
        }

    }
}
