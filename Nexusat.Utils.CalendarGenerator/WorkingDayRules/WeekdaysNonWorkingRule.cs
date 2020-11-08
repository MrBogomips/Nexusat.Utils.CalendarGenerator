using System;
using System.Linq;
using System.Runtime.Serialization;

// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represents a rule based on non working week days
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class WeekdaysNonWorkingRule : DayRule
    {
        [DataMember] private readonly WeekdaysNonWorkingRuleSettings _settings;

        public WeekdaysNonWorkingRule(WeekdaysNonWorkingRuleSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public override DayInfo GetDayInfo(DateTime date)
        {
            TryGetDayInfo(date, out var dayInfo);
            return dayInfo;
        }

        public override bool TryGetDayInfo(DateTime date, out DayInfo dayInfo)
        {
            dayInfo = null;
            var setting = _settings.Days.Where(_ => _.DayOfWeek == date.DayOfWeek);
            if (!setting.Any()) return false;

            dayInfo = new DayInfo();
            return true;
        }
    }
}