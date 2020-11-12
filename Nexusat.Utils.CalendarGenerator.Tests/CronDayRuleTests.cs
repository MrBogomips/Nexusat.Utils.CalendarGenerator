using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class CronDayRuleTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCases), DynamicDataSourceType.Method)]
        public void TestRules(string dayRuleDefinition, DateTime[] matchingDates, DateTime[] unmatchingDates)
        {
            var dayRule = DayRuleParser.Parse(dayRuleDefinition);
            foreach (var @case in matchingDates)
            {
                Assert.IsTrue(dayRule.TryGetDayInfo(@case.Date, out var dayInfo));
                Assert.IsNotNull(dayInfo);
            }
            foreach (var @case in unmatchingDates)
            {
                Assert.IsFalse(dayRule.TryGetDayInfo(@case.Date, out var dayInfo));
                Assert.IsNull(dayInfo);
            }
        }

        public static IEnumerable<object[]> GetCases()
        {
            yield return new object[] {
                "2020 1 1 * # occurs only the 1st January 2020",
                new[] // matching
                {
                    new DateTime(2020, 1, 1)
                },
                new[] // un-matching
                {
                    new DateTime(2020, 1, 2)
                }
            };
            yield return new object[] {
                "2020 1 * * # occurs every day of January 2020",
                new[] // matching
                {
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 5),
                    new DateTime(2020, 1, 30)
                },
                new[] // un-matching
                {
                    new DateTime(2020, 2, 1),
                    new DateTime(2020, 3, 1),
                    new DateTime(2020, 4, 1)
                }
            };
            yield return new object[] {
                "*/Leap 1 1 * # occurs every 1st day of a leap year", 
                new[] // matching
                {
                    new DateTime(2020, 1, 1),
                    new DateTime(2024, 1, 1),
                    new DateTime(2028, 1, 1)
                },
                new[] // un-matching
                {
                    new DateTime(2020, 2, 1),
                    new DateTime(2020, 1, 2),
                    new DateTime(2021, 1, 1),
                    new DateTime(2022, 1, 1),
                    new DateTime(2023, 1, 1)
                }
            };
            yield return new object[] {
                "2020 * * 1..5 # occurs every weekday of the 2020", 
                new[] // matching
                {
                    new DateTime(2020, 11, 2), // monday
                    new DateTime(2020, 11, 3),
                    new DateTime(2020, 11, 4),
                    new DateTime(2020, 11, 5),
                    new DateTime(2020, 11, 6)  // friday
                },
                new[] // un-matching
                {
                    // 2020
                    new DateTime(2020, 11, 7), // saturday
                    new DateTime(2020, 11, 8), // sunday
                    // 2021
                    new DateTime(2021, 11, 2), // monday
                    new DateTime(2021, 11, 3),
                    new DateTime(2021, 11, 4),
                    new DateTime(2021, 11, 5),
                    new DateTime(2021, 11, 6)  // friday
                }
            };
        }
    }
}