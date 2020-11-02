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
    public abstract class CalendarBase : ICalendar
    {
        public string Name { get; private set; }

        public string Description { get; private set; }
        public string LongDescription { get; private set; }

        public CalendarBase(string name, string description = null, string longDescription = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

        }

        #region Equals
        // override object.Equals
        public bool Equals(CalendarBase that) => that != null && that.Name == this.Name;

        public override bool Equals(object that) => Equals(that as CalendarBase);

        // override object.GetHashCode
        public override int GetHashCode() => Name.GetHashCode();
        #endregion Equals
    }
}
