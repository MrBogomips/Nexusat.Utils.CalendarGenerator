﻿using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation

namespace Nexusat.Calendar
{
    public class WeekdaysWorkingRuleBuilder
    {
        private readonly Dictionary<DayOfWeek, List<TimePeriod>> _settings =
            new Dictionary<DayOfWeek, List<TimePeriod>>();

        
        // ReSharper disable once MemberCanBePrivate.Global
        public void AddRule(DayOfWeek dayOfWeek, IEnumerable<TimePeriod> timePeriods)
        {
            if (timePeriods == null) throw new ArgumentNullException(nameof(timePeriods));
            if (_settings.TryGetValue(dayOfWeek, out var curTimePeriods))
            {
                curTimePeriods.AddRange(timePeriods);
            }
            else
            {
                curTimePeriods = new List<TimePeriod>(timePeriods);
                _settings.Add(dayOfWeek, curTimePeriods);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void AddRule(DayOfWeek dayOfWeek, params TimePeriod[] timePeriods) =>
            AddRule(dayOfWeek, timePeriods.ToList());

        public void AddRule(DayOfWeek dayOfWeek, short beginHour, short beginMinute, short endHour, short endMinute) =>
            AddRule(dayOfWeek, new TimePeriod(beginHour, beginMinute, endHour, endMinute));

        // ReSharper disable once UnusedMember.Global
        public void Clear() => _settings.Clear();

        /// <summary>
        /// Retrieve the settings suitable to instantiate a new <see cref="WeekdaysWorkingRule"/>.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public WeekdaysWorkingRuleSettings GetSettings() =>
            new WeekdaysWorkingRuleSettings(_settings.Select(s =>
            new WeekdaysWorkingRuleSettings.DaySetting(s.Key, s.Value)));
        /// <summary>
        /// Retrieve a new <see cref="WeekdaysWorkingRule"/> based of the settings provided so far.
        /// </summary>
        /// <returns></returns>
        public WeekdaysWorkingRule GetRule() => new WeekdaysWorkingRule(GetSettings());
        
    }
}