using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using static System.DayOfWeek;
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Nexusat.Utils.CalendarGenerator.Tests
{

    [TestClass]
    public class CalendarTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private TestContext TestContext { get; set; }

        [TestMethod]
        public void WeekdayWorkingTest()
        {
            // Setup
            var builder = new WeekdaysWorkingRuleBuilder();
            builder.AddRule(Monday, 8, 30, 17, 30);
            builder.AddRule(Tuesday, 8, 30, 17, 30);
            builder.AddRule(Wednesday, 8, 30, 17, 30);
            builder.AddRule(Thursday, 8, 30, 17, 30);
            builder.AddRule(Friday, 8, 30, 17, 30);

            var rules = new DayRules {{DayRulePolicy.Fallthrough, builder.GetRule()}};

            var calendar = new Calendar("Working Calendar", rules);

            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednesday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");

            // Exercise and Assert
            Assert.IsTrue(calendar.GetDayInfo(monday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(tuesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(wednesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(thursday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(friday).IsWorkingDay);

            Assert.IsNull(calendar.GetDayInfo(saturday));
            Assert.IsNull(calendar.GetDayInfo(sunday));

            Assert.IsTrue(calendar.TryGetDayInfo(monday, out var dayInfo));
            Assert.IsNotNull(dayInfo);
            Assert.IsFalse(calendar.TryGetDayInfo(sunday, out dayInfo));
            Assert.IsNull(dayInfo);
        }

        [TestMethod]
        public void WeekdayNonWorkingTest()
        {
            // Setup
            var builder = new WeekdaysNonWorkingRuleBuilder();
            builder.AddRule(Saturday);
            builder.AddRule(Sunday);

            var rules = new DayRules {{DayRulePolicy.Fallthrough, builder.GetRule()}};

            var calendar = new Calendar("NonWorking Calendar", rules);

            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednesday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");

            // Exercise and Assert
            Assert.IsNull(calendar.GetDayInfo(monday));
            Assert.IsNull(calendar.GetDayInfo(tuesday));
            Assert.IsNull(calendar.GetDayInfo(wednesday));
            Assert.IsNull(calendar.GetDayInfo(thursday));
            Assert.IsNull(calendar.GetDayInfo(friday));

            Assert.IsFalse(calendar.GetDayInfo(saturday).IsWorkingDay);
            Assert.IsFalse(calendar.GetDayInfo(sunday).IsWorkingDay);

            Assert.IsFalse(calendar.TryGetDayInfo(monday, out var dayInfo));
            Assert.IsNull(dayInfo);
            Assert.IsTrue(calendar.TryGetDayInfo(sunday, out dayInfo));
            Assert.IsNotNull(dayInfo);
        }

        [TestMethod]
        public void WeekdayTest()
        {
            // Setup
            var calendar = SetupFullCalendar();
            TestFullCalendar(calendar);
        }

        private static void TestFullCalendar(ICalendar calendar)
        {
            var monday = DateTime.Parse("2020-11-02");
            var tuesday = DateTime.Parse("2020-11-03");
            var wednesday = DateTime.Parse("2020-11-04");
            var thursday = DateTime.Parse("2020-11-05");
            var friday = DateTime.Parse("2020-11-06");
            var saturday = DateTime.Parse("2020-11-07");
            var sunday = DateTime.Parse("2020-11-08");


            // Exercise and Assert
            Assert.IsTrue(calendar.GetDayInfo(monday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(tuesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(wednesday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(thursday).IsWorkingDay);
            Assert.IsTrue(calendar.GetDayInfo(friday).IsWorkingDay);
            Assert.IsFalse(calendar.GetDayInfo(saturday).IsWorkingDay);
            Assert.IsFalse(calendar.GetDayInfo(sunday).IsWorkingDay);
        }

        private static Calendar SetupFullCalendar()
        {
            var weekdaysRuleBuilder = new WeekdaysWorkingRuleBuilder();
            weekdaysRuleBuilder.AddRule(Monday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Tuesday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Wednesday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Thursday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Friday, 8, 30, 17, 30);
            weekdaysRuleBuilder.AddRule(Saturday, 8, 30, 17, 30); // This rule would be overriden by the Weekend rules
            weekdaysRuleBuilder.AddRule(Sunday, 8, 30, 17, 30); // This rule would be overriden by the Weekend rules

            var weekendRuleBuilder = new WeekdaysNonWorkingRuleBuilder();
            weekendRuleBuilder.AddRule(Saturday);
            weekendRuleBuilder.AddRule(Sunday);

            var rules = new DayRules
            {
                {DayRulePolicy.Fallthrough, weekdaysRuleBuilder.GetRule()},
                {DayRulePolicy.Fallthrough, weekendRuleBuilder.GetRule()}
            };

            return new Calendar("Five Working Days Calendar", rules);
        }

        [TestMethod]
        public void XmlSerializationTest()
        {
            var calendar = SetupFullCalendar();

            var xml = calendar.ToXml();
            TestContext.WriteLine(xml);
            Assert.IsNotNull(xml);

            var calendar2 = Calendar.LoadFromXml(xml);
            TestFullCalendar(calendar2);

        }

        [TestMethod]
        public void JsonSerializationTest()
        {
            var calendar = SetupFullCalendar();

            var json = calendar.ToJson(indent: true);
            TestContext.WriteLine(json);
            Assert.IsNotNull(json);

            var calendar2 = Calendar.LoadFromJson(json);
            TestFullCalendar(calendar2);
        }

        [TestMethod]
        public void GenerateCalendarDaysTest()
        {
            var calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo
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
            var calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo
            {
                Description = "Default",
                IsWorkingDay = false
            };

            var calendarDays =
                calendar.GenerateCalendarDays(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), defaultDayInfo);
            var xmlSettings = new XmlWriterSettings {Indent = true};
            var xml = calendarDays.ToXml(xmlSettings);
            Assert.IsNotNull(xml);
            TestContext.WriteLine(xml);
        }
    }
}
