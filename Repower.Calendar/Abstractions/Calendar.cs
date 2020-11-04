using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Repower.Calendar
{
    /// <summary>
    /// Base implementation of a calendar.
    /// 
    /// See <see cref="ICalendar"/> for the semantic of the members.
    /// </summary>
    public abstract class Calendar : ICalendar
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
        /// <returns><c>null</c> if the calendar has no rules</returns>
        public IWorkingDayInfo GetWorkingDayInfo(DateTime date)
        {
            IWorkingDayInfo retVal = null;
            if (WorkingDayRules != null)
            {
                foreach (var rule in WorkingDayRules)
                {
                    retVal = rule.WorkingDayRule.GetWorkingDayInfo(date);
                    if (retVal.IsWorkingDay && rule.WorkingDayPolicy == WorkingDayRulePolicy.AcceptIfTrue ||
                        !retVal.IsWorkingDay && rule.WorkingDayPolicy == WorkingDayRulePolicy.AcceptIfFalse)
                    {
                        break;
                    }
                }
            }
            return retVal;
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
