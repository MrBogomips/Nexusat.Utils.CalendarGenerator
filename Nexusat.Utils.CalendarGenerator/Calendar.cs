using System;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation

namespace Nexusat.Utils.CalendarGenerator
{
    // TODO: NuGet Packaging
    /// <summary>
    ///     Base implementation of a calendar.
    ///     See <see cref="ICalendar" /> for the semantic of the members.
    /// </summary>
    public class Calendar : ICalendar
    {
        public Calendar(string name, CalendarRules calendarRules, string description = null,
            string longDescription = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            LongDescription = longDescription;
            CalendarRules = calendarRules ?? throw new ArgumentNullException(nameof(calendarRules));
        }

        public static Calendar EmptyCalendar { get; } = new Calendar("EmptyCalendar", new CalendarRules());

        public CalendarRules CalendarRules { get; }

        public string Name { get; }

        public string Description { get; }

        public string LongDescription { get; }

        /// <summary>
        ///     Return the working day info of the day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns><c>null</c> if the calendar has no workingDayInfo</returns>
        public DayInfo GetDayInfo(DateTime date)
        {
            TryGetDayInfo(date, out var dayInfo);
            return dayInfo;
        }

        public bool TryGetDayInfo(DateTime date, out DayInfo dayInfo)
        {
            dayInfo = null;
            if (CalendarRules == null || !CalendarRules.Any()) return false; // no rules => nothing to do

            foreach (var rule in CalendarRules)
            {
                if (!rule.Rule.TryGetDayInfo(date, out var curInfo)) continue; // something to evaluate
                dayInfo = curInfo; // register the most recent found
                if (rule.Policy == DayRulePolicy.Accept) break;
            }

            return dayInfo != null;
        }

        /// <summary>
        ///     Generate a <see cref="CalendarDays" /> info.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="defaultDayInfo">Default info in case of missing info</param>
        /// <returns></returns>
        public CalendarDays GenerateCalendarDays(DateTime from, DateTime to, DayInfo defaultDayInfo)
        {
            if (to < from) throw new ArgumentException($"'{nameof(to)}' cannot precede '{nameof(from)}'");

            if (defaultDayInfo is null) throw new ArgumentNullException(nameof(defaultDayInfo));

            var calendarDays = new CalendarDays();
            for (var cur = from; cur <= to; cur = cur.AddDays(1))
            {
                var info = GetDayInfo(cur) ?? defaultDayInfo;
                var workingPeriods =
                    info.WorkingPeriods?.Select(wp => new CalendarDays.TimePeriod
                    {
                        Begin = wp.Begin.ToString(),
                        End = wp.End.ToString()
                    }).ToList();

                calendarDays.Add(new CalendarDays.Day
                {
                    Date = cur.ToString("yyyy-MM-dd"),
                    IsWorkingDay = info.IsWorkingDay,
                    Description = info.Description,
                    WorkingPeriods = workingPeriods
                });
            }

            return calendarDays;
        }

        /// <summary>
        ///     Add rules to the collection of rules
        /// </summary>
        /// <param name="rules"></param>
        // ReSharper disable once UnusedMember.Global
        public void AddRules(CalendarRules rules) => CalendarRules.AddRange(rules);

        /// <summary>
        ///     Add the rules from the calendar to the collection of rules
        /// </summary>
        /// <param name="calendar"></param>
        public void AddRules(Calendar calendar) => CalendarRules.Add(calendar);
    }
}