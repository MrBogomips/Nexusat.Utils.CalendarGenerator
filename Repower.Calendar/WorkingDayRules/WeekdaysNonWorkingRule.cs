using System;
using System.Linq;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a rule based on non working week days
    /// </summary>
    public class WeekdaysNonWorkingRule : IDayRule
    {
        internal static readonly string NAME = "NonWorkingWeekdays";
        public string Name => NAME;

        private readonly WeekdaysNonWorkingRuleSettings Settings;

        public WeekdaysNonWorkingRule(WeekdaysNonWorkingRuleSettings settings) =>
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

        public IDayInfo GetDayInfo(DateTime date)
        {
            IDayInfo dayInfo;
            TryGetDayInfo(date, out dayInfo);
            return dayInfo;
        }

        public bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo)
        {
            dayInfo = null;
            var setting = Settings.Days.Where((d) => d.DayOfWeek == date.DayOfWeek);
            if (setting == null || !setting.Any())
            {
                return false;
            }
            else
            {
                dayInfo = new DayInfo { IsWorkingDay = false };
                return true;
            }
        }
    }
}
