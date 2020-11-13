using System;
using System.Diagnostics;
using Nexusat.Utils.CalendarGenerator;

/*************************************************
 * SETTING UP CALENDARS
 */

var calendarStandardWorkingWeekRules = new CalendarRules
{
    // In the following rule we provide a list of working periods, therefore the matching days will be marked as working days
    // Fallthrouch policy don't stop the evaluation chain
    {DayRulePolicy.Fallthrough, DayRuleParser.Parse("* * * * [[working day]] 08:30-13:30,14:30-17:30")},
    // In the following rule we override the weekends as non-working days by the fact we don't provide a working period
    {DayRulePolicy.Fallthrough, DayRuleParser.Parse("* * * 6..7 [[non working day]]")}
};

// We build a simple calendar on these rules
var workingWeek = new Calendar("Standard working week", calendarStandardWorkingWeekRules);

var calendarItalianHolydaysRules = new CalendarRules
{
    // In the following rule we state that all 25th of december are xmas days and the policy we'll force to accept this outcome
    {DayRulePolicy.AcceptAlways, DayRuleParser.Parse("* 12 25 * [[xmas]]")},
    // Same as before
    {DayRulePolicy.AcceptAlways, DayRuleParser.Parse("* 1 1 * [[new year]]")}
};

// We buils a simple calendar for italian holydays
var italianHolydays = new Calendar("Italian Holidays", calendarItalianHolydaysRules);

/*************************************************
 * CHECKING DAYS
 */
var aMonday = DateTime.Parse("2020-12-21");
var aXmasDay = DateTime.Parse("2020-12-25"); //it's a friday
var aSaturday = DateTime.Parse("2020-12-26");

var dayInfo = workingWeek.GetDayInfo(aMonday);
Debug.Assert(dayInfo is not null, "The calendar definition will always provide a dayInfo");
Debug.Assert(dayInfo.IsWorkingDay, "For the working calendar every monday is a working day");
dayInfo = workingWeek.GetDayInfo(aXmasDay);
Debug.Assert(dayInfo is not null, "The calendar definition will always provide a dayInfo");
Debug.Assert(dayInfo.IsWorkingDay, "For the working calendar every monday is a working day");
dayInfo = workingWeek.GetDayInfo(aSaturday);
Debug.Assert(dayInfo is not null, "The calendar definition will always provide a dayInfo");
Debug.Assert(!dayInfo.IsWorkingDay, "For the working calendar every saturday is not a working day");

/****************************************************
 * COMBINING CALENDARS' RULES
 */
// You can also combine multiple calendars' rules.
// This is useful when you have to manage general purpose calendar shared amongst more specific calendars
workingWeek.AddRules(italianHolydays);
dayInfo = workingWeek.GetDayInfo(aXmasDay);
Debug.Assert(dayInfo is not null, "The calendar definition will always provide a dayInfo");
Debug.Assert(!dayInfo.IsWorkingDay, "Now the xmas is considered a non working day");

/***************************************************
 * GENERATE CALENDAR TABLES
 */
var defaultDayInfo = new DayInfo(); // We don't require a default dayInfo in this scenario because our calendar rules will always provide DayInfo data
var december2020days = workingWeek.GenerateCalendarDays(DateTime.Parse("2020-12-01"), DateTime.Parse("2020-12-31"), defaultDayInfo);
Console.WriteLine(december2020days.ToJson(true));

/**************************************************
 * SERIALIZE CALENDAR DEFINITIONS (ONLY XML DEMO)
 */
var xmlCalendar = workingWeek.ToXml();
Console.WriteLine(xmlCalendar);
var newCalendar = CalendarSerializer.LoadFromXml(xmlCalendar);
Console.WriteLine(newCalendar.Name);


