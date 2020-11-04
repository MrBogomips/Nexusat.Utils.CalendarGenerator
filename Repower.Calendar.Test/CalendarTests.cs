using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repower.Calendar;
using System;
using System.Collections.Generic;
using System.Text;

using static System.DayOfWeek;

namespace Repower.Calendar.Tests
{
    [TestClass()]
    public class CalendarTests
    {
        [TestMethod()]
        public void WeekdayTest()
        {
            // Setup
            WeekdaysRuleBuilder builder = new WeekdaysRuleBuilder();
            builder.AddRule(Monday, 8, 30, 17, 30);
            builder.AddRule(Tuesday, 8, 30, 17, 30);
            builder.AddRule(Wednesday, 8, 30, 17, 30);
            builder.AddRule(Thursday, 8, 30, 17, 30);
            builder.AddRule(Friday, 8, 30, 17, 30);

            WorkingDayRules rules = new WorkingDayRules();
            rules.Add(WorkingDayRulePolicy.Fallthrough, builder.GetRule());

            var calendar = new Calendar("Weekdays", rules);

            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednsday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");

            // Excercise and Assert
            Assert.IsTrue(calendar.GetWorkingDayInfo(monday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(tuesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(wednsday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(thursday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(friday).IsWorkingDay);

            Assert.IsNull(calendar.GetWorkingDayInfo(saturday));
            Assert.IsNull(calendar.GetWorkingDayInfo(sunday));

            IWorkingDayInfo dayInfo;
            
            Assert.IsTrue(calendar.TryGetWorkingDayInfo(monday, out dayInfo));
            Assert.IsNotNull(dayInfo);
            Assert.IsFalse(calendar.TryGetWorkingDayInfo(sunday, out dayInfo));
            Assert.IsNull(dayInfo);
        }
    }
}