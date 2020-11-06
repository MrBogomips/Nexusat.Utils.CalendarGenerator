using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Calendar;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;
using static System.DayOfWeek;
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Calendar.Tests
{

    [TestClass()]
    public class CalendarTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        private TestContext TestContext { get; set; }

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

            DayRules rules = new DayRules();
            rules.Add(DayRulePolicy.Fallthrough, builder.GetRule());

            var calendar = new Calendar("Working Calendar", rules);

            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednsday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");

            // Excercise and Assert
            Assert.IsTrue(calendar.GetDayInfo(monday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(tuesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(wednsday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(thursday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(friday).IsWorkingDay);

            Assert.IsNull(calendar.GetDayInfo(saturday));
            Assert.IsNull(calendar.GetDayInfo(sunday));

            IDayInfo dayInfo;
            
            Assert.IsTrue(calendar.TryGetDayInfo(monday, out dayInfo));
            Assert.IsNotNull(dayInfo);
            Assert.IsFalse(calendar.TryGetDayInfo(sunday, out dayInfo));
            Assert.IsNull(dayInfo);
        }

        [TestMethod]
        public void WeekdayNonWorkingTest()
        {
            // Setup
            WeekdaysNonWorkingRuleBuilder builder = new WeekdaysNonWorkingRuleBuilder();
            builder.AddRule(Saturday);
            builder.AddRule(Sunday);
            
            DayRules rules = new DayRules();
            rules.Add(DayRulePolicy.Fallthrough, builder.GetRule());

            var calendar = new Calendar("NonWorking Calendar", rules);

            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednsday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");

            // Excercise and Assert
            Assert.IsNull(calendar.GetDayInfo(monday));
            Assert.IsNull(calendar.GetDayInfo(tuesday));
            Assert.IsNull(calendar.GetDayInfo(wednsday));
            Assert.IsNull(calendar.GetDayInfo(thursday));
            Assert.IsNull(calendar.GetDayInfo(friday));

            Assert.IsFalse(calendar.GetDayInfo(saturday).IsWorkingDay);
            Assert.IsFalse(calendar.GetDayInfo(sunday).IsWorkingDay);

            IDayInfo dayInfo;

            Assert.IsFalse(calendar.TryGetDayInfo(monday, out dayInfo));
            Assert.IsNull(dayInfo);
            Assert.IsTrue(calendar.TryGetDayInfo(sunday, out dayInfo));
            Assert.IsNotNull(dayInfo);
        }

        [TestMethod]
        public void WeekdayTest()
        {
            // Setup
            Calendar calendar = SetupFullCalendar();
            TestFullCalendar(calendar);
        }

        private static void TestFullCalendar(Calendar calendar)
        {
            DateTime monday, tuesday, wednsday, thursday, friday, saturday, sunday;
            monday = DateTime.Parse("2020-11-02");
            tuesday = DateTime.Parse("2020-11-03");
            wednsday = DateTime.Parse("2020-11-04");
            thursday = DateTime.Parse("2020-11-05");
            friday = DateTime.Parse("2020-11-06");
            saturday = DateTime.Parse("2020-11-07");
            sunday = DateTime.Parse("2020-11-08");


            // Excercise and Assert
            Assert.IsTrue(calendar.GetDayInfo(monday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(tuesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(wednsday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(thursday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(friday).IsWorkingDay);
            Assert.IsFalse(calendar.GetDayInfo(saturday).IsWorkingDay);
            Assert.IsFalse(calendar.GetDayInfo(sunday).IsWorkingDay);
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

            DayRules rules = new DayRules();
            rules.Add(DayRulePolicy.Fallthrough, weekdaysRuleBuilder.GetRule());
            rules.Add(DayRulePolicy.Fallthrough, weekendRuleBuilder.GetRule());

            return new Calendar("Five Working Days Calendar", rules);
        }

        [TestMethod]
        public void XmlSerializationTest()
        {
            Calendar calendar = SetupFullCalendar();

            var xml = calendar.ToXml();
            TestContext.WriteLine(xml);
            Assert.IsNotNull(xml);

            var calendar2 = Calendar.LoadFromXml(xml);
            TestFullCalendar(calendar2);

        }

        [TestMethod]
        public void JsonSerializationTest()
        {
            Calendar calendar = SetupFullCalendar();

            var json = calendar.ToJson(indent: true);
            TestContext.WriteLine(json);
            Assert.IsNotNull(json);

            var calendar2 = Calendar.LoadFromJson(json);
            TestFullCalendar(calendar2);
        }

        [TestMethod]
        public void GenerateCalendarDaysTest()
        {
            Calendar calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo()
            {
                Description = "Default",
                IsWorkingDay = false
            };

            var calendarDays = calendar.GenerateCalendarDays(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), defaultDayInfo);

            Assert.IsNotNull(calendarDays);
            Assert.AreEqual(366, calendarDays.Count);
        }
        [TestMethod]
        public void CalendarDaysToXmlTest()
        {
            Calendar calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo()
            {
                Description = "Default",
                IsWorkingDay = false
            };

            var calendarDays = calendar.GenerateCalendarDays(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), defaultDayInfo);
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            var xml = calendarDays.ToXml(xmlSettings);
            Assert.IsNotNull(xml);
            TestContext.WriteLine(xml);
        }

       
    }
}