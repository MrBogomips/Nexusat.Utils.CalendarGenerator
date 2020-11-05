using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a rule based on non working week days
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class WeekdaysNonWorkingRule : DayRule
    {
        [DataMember]
        private readonly WeekdaysNonWorkingRuleSettings Settings;

        public WeekdaysNonWorkingRule(WeekdaysNonWorkingRuleSettings settings) =>
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

        public override IDayInfo GetDayInfo(DateTime date)
        {
            IDayInfo dayInfo;
            TryGetDayInfo(date, out dayInfo);
            return dayInfo;
        }

        public override bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo)
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
