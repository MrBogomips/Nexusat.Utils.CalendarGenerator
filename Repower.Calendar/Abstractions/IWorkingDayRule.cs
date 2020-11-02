using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repower.Calendar
{

    public interface IWorkingDayRule: IWorkingDayInfoProvider
    {
        /// <summary>
        /// The public name of the rule
        /// </summary>
        string Name { get; }
    }
}
