using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable HeapView.ObjectAllocation.Evident
// TODO: migrate to xUnit
namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class CalendarTests
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once MemberCanBePrivate.Global
        public TestContext TestContext { get; set; }



        

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
            throw new NotImplementedException();
            /*
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
            */
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
        public void EmptyCalendarTest()
        {
            var calendar = Calendar.EmptyCalendar;
            Assert.IsNotNull(calendar);
            Assert.AreSame(Calendar.EmptyCalendar, calendar, "Empty calendar is a singleton");
            var xml = calendar.ToXml();
            TestContext.WriteLine($"Calendar XML Definition:\n{xml}");
            Assert.IsNull(calendar.GetDayInfo(DateTime.Now), "Empty calendar doesn't provide any info");
            var defaultDayInfo = new DayInfo();

            var days = calendar.GenerateCalendarDays(
                new DateTime(2020, 1, 1),
                new DateTime(2020, 12, 31), defaultDayInfo);
            Assert.AreEqual(366, days.Count);
            xml = days.ToXml();
            Assert.IsNotNull(xml);
            TestContext.WriteLine($"\n\nCalendar Days:\n{xml}");
        }

        [TestMethod]
        public void JsonSerializationTest()
        {
            var calendar = SetupFullCalendar();

            var json = calendar.ToJson(true);
            TestContext.WriteLine(json);
            Assert.IsNotNull(json);

            var calendar2 = Calendar.LoadFromJson(json);
            TestFullCalendar(calendar2);
        }

        [TestMethod]
        public void GenerateCalendarDaysTest()
        {
            var calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo("Default");

            var calendarDays =
                calendar.GenerateCalendarDays(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), defaultDayInfo);

            Assert.IsNotNull(calendarDays);
            Assert.AreEqual(366, calendarDays.Count);
        }

        [TestMethod]
        public void CalendarDaysToXmlTest()
        {
            var calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo("info");

            var calendarDays =
                calendar.GenerateCalendarDays(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), defaultDayInfo);
            var xmlSettings = new XmlWriterSettings {Indent = true};
            var xml = calendarDays.ToXml(xmlSettings);
            Assert.IsNotNull(xml);
            TestContext.WriteLine(xml);
        }

        [TestMethod]
        public void CalendarDaysToJsonTest()
        {
            var calendar = SetupFullCalendar();

            var defaultDayInfo = new DayInfo("info");

            var calendarDays =
                calendar.GenerateCalendarDays(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), defaultDayInfo);
            var json = calendarDays.ToJson(true);
            Assert.IsNotNull(json);
            TestContext.WriteLine(json);
        }

        [TestMethod]
        public void CalendarAddRulesTest()
        {
            var calendar = SetupFullCalendar();
            var calendar2 = SetupFullCalendar();
            var ruleCount = calendar.DayRules.Count;
            calendar.AddRules(calendar2);
            Assert.AreEqual(ruleCount * 2, calendar.DayRules.Count);
            TestContext.WriteLine($"Calendar XML:\n{calendar.ToXml(new XmlWriterSettings {Indent = true})}");
        }
    }
}