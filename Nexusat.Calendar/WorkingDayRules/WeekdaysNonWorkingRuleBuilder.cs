using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexusat.Calendar
{
    public class WeekdaysNonWorkingRuleBuilder
    {
        private HashSet<DayOfWeek> settings = new HashSet<DayOfWeek>();

        public void AddRule(DayOfWeek dayOfWeek) => settings.Add(dayOfWeek);

        public void Clear() => settings.Clear();

        /// <summary>
        /// Retrieve the settings suitable to instantiate a new <see cref="WeekdaysWorkingRule"/>.
        /// </summary>
        /// <returns></returns>
        public WeekdaysNonWorkingRuleSettings GetSettings() =>
            new WeekdaysNonWorkingRuleSettings(settings.Select( s => new WeekdaysNonWorkingRuleSettings.DaySetting(s)));
        /// <summary>
        /// Retrieve a new <see cref="WeekdaysWorkingRule"/> based of the settings provided so far.
        /// </summary>
        /// <returns></returns>
        public WeekdaysNonWorkingRule GetRule() => new WeekdaysNonWorkingRule(GetSettings());

    }
}
