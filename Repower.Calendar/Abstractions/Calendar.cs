using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Repower.Calendar
{
    /// <summary>
    /// Base implementation of a calendar.
    /// 
    /// See <see cref="ICalendar"/> for the semantic of the members.
    /// </summary>
    public class Calendar : ICalendar
    {
        private readonly WorkingDayRules WorkingDayRules;

        public string Name { get; private set; }

        public string Description { get; private set; }
        public string LongDescription { get; private set; }

        public Calendar(string name, WorkingDayRules workingDayRules, string description = null, string longDescription = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            WorkingDayRules = workingDayRules ?? throw new ArgumentNullException(nameof(workingDayRules));
        }

        /// <summary>
        /// Return the working day info of the day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns><c>null</c> if the calendar has no workingDayInfo</returns>
        public IWorkingDayInfo GetWorkingDayInfo(DateTime date)
        {
            IWorkingDayInfo dayInfo;
            TryGetWorkingDayInfo(date, out dayInfo);
            return dayInfo;
        }
        public bool TryGetWorkingDayInfo(DateTime date, out IWorkingDayInfo dayInfo)
        {
            dayInfo = null;
            if (WorkingDayRules == null || !WorkingDayRules.Any())
            {
                return false; // no rules => nothing to do
            }
            else
            {
                foreach (var rule in WorkingDayRules)
                {
                    IWorkingDayInfo curInfo;

                    if (rule.Rule.TryGetWorkingDayInfo(date, out curInfo))
                    { // something to evaluate
                        dayInfo = curInfo; // register the most recent found
                        if (rule.Policy == WorkingDayRulePolicy.AcceptAlways ||
                            dayInfo.IsWorkingDay && rule.Policy == WorkingDayRulePolicy.AcceptIfTrue ||
                            !dayInfo.IsWorkingDay && rule.Policy == WorkingDayRulePolicy.AcceptIfFalse)
                        {
                            break;
                        }
                    }
                }
            }
            return dayInfo != null;
        }

        #region Equals
        // override object.Equals
        public bool Equals(Calendar that) => that != null && that.Name == this.Name;

        public override bool Equals(object that) => Equals(that as Calendar);

        // override object.GetHashCode
        public override int GetHashCode() => Name.GetHashCode();

        #endregion Equals
    }
}
