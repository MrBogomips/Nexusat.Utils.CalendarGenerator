using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    /// <summary>
    /// Parse a day rule expression
    ///
    /// A Day Rule Expression is:
    ///
    /// * * * * [[{description}]] {working periods}  #{comments}
    /// ^ ^ ^ ^ ^                 ^                  ^
    /// | | | | |                 |                  |
    /// | | | | |                 |                  +- (Optional) EoL comment
    /// | | | | |                 |
    /// | | | | |                 +-------------------- (Optional) Comma separated list of working periods
    /// | | | | |                    
    /// | | | | +-------------------------------------- (Optional) Description of the day
    /// | | | |
    /// | | | +---------------------------------------- Day of Week rule
    /// | | |
    /// | | +------------------------------------------ Day of Month rule
    /// | |
    /// | +-------------------------------------------- Month rule
    /// |
    /// +---------------------------------------------- Year rule
    /// <example>
    /// Every day non working day: * * * * [[]]
    /// Every day working day: * * * * [[]] 08:30-17:30
    /// Vacancy on 29th of february: * * 29 * * [[vacancy on the extra day]]
    /// Vacancy on 1st of march of leap years: */Leap 3 1 * [[Vacancy on the extra day]]
    /// Ordinary working days: * * * 1..5 [[]] 08:30-13:30,14:30-17:30
    /// </example>
    /// </summary>
    [TestClass]
    public class CronDayRuleParserTests
    {
        [TestMethod]
        public void InvalidParseTests()
        {
            var value = @"";
            Assert.IsFalse(CronDayRuleParser.Instance.TryParse(value, out var obj));
            value = null;
            Assert.IsFalse(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "  ";
            Assert.IsFalse(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "*";
            Assert.IsFalse(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "* *";
            Assert.IsFalse(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "* * *";
            Assert.IsFalse(CronDayRuleParser.Instance.TryParse(value, out obj));
        }

        [TestMethod]
        public void ValidParseTests()
        {
            // Basic
            var value = "* * * *"; // No description and working periods
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out var obj));
            value = "* * * * [[]]"; // No working periods
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "* * * * 08:00-12:00"; // No description
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            
            // Basic with comments
            value = "* * * * # über"; 
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "* * * * [[]] # über"; // No working periods
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            value = "* * * * 08:00-12:00 # über"; // No description
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
        }

        [TestMethod]
        public void YearMatcherTest()
        {
            var value = "*,*%2,1../2,*/Leap,*/NotLeap * * *";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out var obj));
            Assert.AreEqual(5, obj.YearMatchers.Count);
            Assert.IsInstanceOfType(obj.YearMatchers[0], typeof(RangeYearMatcher));
            Assert.IsInstanceOfType(obj.YearMatchers[1], typeof(ModuloYearMatcher));
            Assert.IsInstanceOfType(obj.YearMatchers[2], typeof(PeriodicYearMatcher));
            Assert.IsInstanceOfType(obj.YearMatchers[3], typeof(LeapYearMatcher));
            Assert.IsInstanceOfType(obj.YearMatchers[4], typeof(NotLeapYearMatcher));
        }
        
        [TestMethod]
        public void MonthMatcherTest()
        {
            var value = "* *,*%2,1../2 * *";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out var obj));
            Assert.AreEqual(3, obj.MonthMatchers.Count);
            Assert.IsInstanceOfType(obj.MonthMatchers[0], typeof(RangeMonthMatcher));
            Assert.IsInstanceOfType(obj.MonthMatchers[1], typeof(ModuloMonthMatcher));
            Assert.IsInstanceOfType(obj.MonthMatchers[2], typeof(PeriodicMonthMatcher));
        }
        
        [TestMethod]
        public void DayOfMonthMatcherTest()
        {
            var value = "* * *,*%2,1../2 *";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out var obj));
            Assert.AreEqual(3, obj.DayOfMonthMatchers.Count);
            Assert.IsInstanceOfType(obj.DayOfMonthMatchers[0], typeof(RangeDayOfMonthMatcher));
            Assert.IsInstanceOfType(obj.DayOfMonthMatchers[1], typeof(ModuloDayOfMonthMatcher));
            Assert.IsInstanceOfType(obj.DayOfMonthMatchers[2], typeof(PeriodicDayOfMonthMatcher));
        }
        
        [TestMethod]
        public void DayOfWeekMatcherTest()
        {
            var value = "* * * 1,3..6";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out var obj));
            Assert.AreEqual(2, obj.DayOfWeekMatchers.Count);
            Assert.IsInstanceOfType(obj.DayOfWeekMatchers[0], typeof(RangeDayOfWeekMatcher));
            Assert.IsInstanceOfType(obj.DayOfWeekMatchers[1], typeof(RangeDayOfWeekMatcher));
        }
        
        [TestMethod]
        public void DescriptionAndWorkingPeriodsTest()
        {
            var value = "* * * *";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out var obj));
            Assert.IsNull((obj.Description));
            Assert.IsNull(obj.WorkingPeriods);
            
            value = "* * * * [[]]";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            Assert.IsNull((obj.Description));
            Assert.IsNull(obj.WorkingPeriods);
            
            value = "* * * * [[description]]";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            Assert.AreEqual("description",obj.Description);
            Assert.IsNull(obj.WorkingPeriods);
            
            value = "* * * * 04:00-05:00,02:00-03:00";
            Assert.IsTrue(CronDayRuleParser.Instance.TryParse(value, out obj));
            Assert.IsNull((obj.Description));
            Assert.IsNotNull(obj.WorkingPeriods);
            Assert.AreEqual(2, obj.WorkingPeriods.Count);
            Assert.AreEqual(TimePeriod.Parse("04:00-05:00"), obj.WorkingPeriods[0]);
            Assert.AreEqual(TimePeriod.Parse("02:00-03:00"), obj.WorkingPeriods[1]);
        }
    }
}