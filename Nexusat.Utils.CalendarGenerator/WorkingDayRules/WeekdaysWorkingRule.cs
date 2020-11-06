using System;
using System.Linq;
using System.Runtime.Serialization;
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable PossibleMultipleEnumeration

namespace Nexusat.Utils.CalendarGenerator
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
            TryGetDayInfo(date, out var dayInfo);
            return dayInfo;
        }

        public override bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo)
        {
            dayInfo = null;
            var setting = Settings.Days.Where(_ => _.DayOfWeek == date.DayOfWeek);
            if (!setting.Any())
            {
                return false;
            }

            dayInfo = new DayInfo { IsWorkingDay = true, WorkingPeriods = setting.First().WorkingPeriods };
            return true;
        }
    }
}
