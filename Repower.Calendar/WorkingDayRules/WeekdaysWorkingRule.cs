using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Repower.Calendar
{
    /// <summary>
    /// Represents a rule based on working week days
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class WeekdaysWorkingRule : DayRule
    {
        [DataMember]
        public readonly WeekdaysWorkingRuleSettings Settings;


        public WeekdaysWorkingRule(WeekdaysWorkingRuleSettings settings) =>
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
                dayInfo = new DayInfo { IsWorkingDay = true, WorkingPeriods = setting.First().WorkingPeriods };
                return true;
            }
        }
    }
}
