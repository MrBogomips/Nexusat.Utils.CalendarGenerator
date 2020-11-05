using System;
using System.Collections.Generic;
using System.Text;

namespace Repower.Calendar.Serialization
{
    [Serializable]
    public class Calendar
    {
        public DayRules WorkingDayRules { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string LongDescription { get; set; }


    }
}
