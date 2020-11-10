using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation

namespace Nexusat.Utils.CalendarGenerator
{
    public class WeekdaysNonWorkingRuleBuilder
    {
        private readonly HashSet<System.DayOfWeek> _settings = new HashSet<System.DayOfWeek>();

        public void AddRule(System.DayOfWeek dayOfWeek)
        {
            _settings.Add(dayOfWeek);
        }

        // ReSharper disable once UnusedMember.Global
        public void Clear()
        {
            _settings.Clear();
        }

        /// <summary>
        ///     Retrieve the settings suitable to instantiate a new <see cref="WeekdaysWorkingRule" />.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public WeekdaysNonWorkingRuleSettings GetSettings()
        {
            return new WeekdaysNonWorkingRuleSettings(_settings.Select(s =>
                new WeekdaysNonWorkingRuleSettings.DaySetting(s)));
        }

        /// <summary>
        ///     Retrieve a new <see cref="WeekdaysWorkingRule" /> based of the settings provided so far.
        /// </summary>
        /// <returns></returns>
        public WeekdaysNonWorkingRule GetRule()
        {
            return new WeekdaysNonWorkingRule(GetSettings());
        }
    }
}