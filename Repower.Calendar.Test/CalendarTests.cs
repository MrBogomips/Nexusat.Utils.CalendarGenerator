using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repower.Calendar;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using static System.DayOfWeek;

namespace Repower.Calendar.Tests
{
    [TestClass()]
    public class CalendarTests
    {
        [TestMethod()]
        public void WeekdayWorkingTest()
        {
            // Setup
            WeekdaysWorkingRuleBuilder builder = new WeekdaysWorkingRuleBuilder();
            builder.AddRule(Monday, 8, 30, 17, 30);
            builder.AddRule(Tuesday, 8, 30, 17, 30);
            builder.AddRule(Wednesday, 8, 30, 17, 30);
            builder.AddRule(Thursday, 8, 30, 17, 30);
            builder.AddRule(Friday, 8, 30, 17, 30);

            WorkingDayRules rules = new WorkingDayRules();
            rules.Add(WorkingDayRulePolicy.Fallthrough, builder.GetRule());

            var calendar = new Calendar("Working Calendar", rules);

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

        [TestMethod]
        public void WeekdayNonWorkingTest()
        {
            // Setup
            WeekdaysNonWorkingRuleBuilder builder = new WeekdaysNonWorkingRuleBuilder();
            builder.AddRule(Saturday);
            builder.AddRule(Sunday);
            
            WorkingDayRules rules = new WorkingDayRules();
            rules.Add(WorkingDayRulePolicy.Fallthrough, builder.GetRule());

            var calendar = new Calendar("NonWorking Calendar", rules);

            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednsday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");

            // Excercise and Assert
            Assert.IsNull(calendar.GetWorkingDayInfo(monday));
            Assert.IsNull(calendar.GetWorkingDayInfo(tuesday));
            Assert.IsNull(calendar.GetWorkingDayInfo(wednsday));
            Assert.IsNull(calendar.GetWorkingDayInfo(thursday));
            Assert.IsNull(calendar.GetWorkingDayInfo(friday));

            Assert.IsFalse(calendar.GetWorkingDayInfo(saturday).IsWorkingDay);
            Assert.IsFalse(calendar.GetWorkingDayInfo(sunday).IsWorkingDay);

            IWorkingDayInfo dayInfo;

            Assert.IsFalse(calendar.TryGetWorkingDayInfo(monday, out dayInfo));
            Assert.IsNull(dayInfo);
            Assert.IsTrue(calendar.TryGetWorkingDayInfo(sunday, out dayInfo));
            Assert.IsNotNull(dayInfo);
        }

        [TestMethod]
        public void WeekdayTest()
        {
            // Setup
            Calendar calendar = SetupFullCalendar();

            DateTime monday, tuesday, wednsday, thursday, friday, saturday, sunday;
            monday = DateTime.Parse("2020-11-02");
            tuesday = DateTime.Parse("2020-11-03");
            wednsday = DateTime.Parse("2020-11-04");
            thursday = DateTime.Parse("2020-11-05");
            friday = DateTime.Parse("2020-11-06");
            saturday = DateTime.Parse("2020-11-07");
            sunday = DateTime.Parse("2020-11-08");
            

            // Excercise and Assert
            Assert.IsTrue(calendar.GetWorkingDayInfo(monday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(tuesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(wednsday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(thursday).IsWorkingDay);
            Assert.IsTrue(calendar.GetWorkingDayInfo(friday).IsWorkingDay);
            Assert.IsFalse(calendar.GetWorkingDayInfo(saturday).IsWorkingDay);
            Assert.IsFalse(calendar.GetWorkingDayInfo(sunday).IsWorkingDay);
        }

        private static Calendar SetupFullCalendar()
        {
            WeekdaysWorkingRuleBuilder weekdaysRuleBuilder = new WeekdaysWorkingRuleBuilder();
            weekdaysRuleBuilder.AddRule(Monday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Tuesday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Wednesday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Thursday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Friday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Saturday, 8, 30, 17, 30); // This rule would be overriden by the Weekend rules
            weekdaysRuleBuilder.AddRule(Sunday, 8, 30, 17, 30); // This rule would be overriden by the Weekend rules

            WeekdaysNonWorkingRuleBuilder weekendRuleBuilder = new WeekdaysNonWorkingRuleBuilder();
            weekendRuleBuilder.AddRule(Saturday);
            weekendRuleBuilder.AddRule(Sunday);

            WorkingDayRules rules = new WorkingDayRules();
            rules.Add(WorkingDayRulePolicy.Fallthrough, weekdaysRuleBuilder.GetRule());
            rules.Add(WorkingDayRulePolicy.Fallthrough, weekendRuleBuilder.GetRule());

            return new Calendar("Five Working Days Calendar", rules);
        }

        [TestMethod]
        public void XmlSerializationTest()
        {
            Calendar calendar = SetupFullCalendar();

            var xml = calendar.ToXml();
            Assert.IsNotNull(xml);

        }
    }
}