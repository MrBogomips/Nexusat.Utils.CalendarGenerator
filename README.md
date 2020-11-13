# Nexusat.Utils.CalendarGenerator

.NET Library to generate a perpetual calendar based on rules.

## Purpose

The main purpose of this library is to simplify the generation of calendar days tables.

More in details with this libary you can:
* define calendars by a succint syntax
* combine calendars in a hierarchy
* check if a date match a calendar and fetch day info
* generate calendar days tables in JSON or XML formats
* serialize and deserialize calendar definition in JSON or XML, useful to persist a calendar on an external system (DB or filesystem)

## Calendar Definition

The definition of a achieved by specifing a set of rules.

Each rule is made up of the following parts:
* An evaluation policy
* A date matcher expression
* A description of the day (optional)
* A list of working day periods (optional)

### Evaluation Policies

|Policy         | Description
|---------------|---------------------------------------------------------------------------
|**Fallthrough**| The rule is accepted but the evaluation continues with the following rules 
|**Always**     | The rule is accepted and the evaluation stops

### Date Matcher Expression (aka Date Pattern)

Date matcher pattern is inspired by [CRON expressions](https://en.wikipedia.org/wiki/Cron#CRON_expression).
They are made up of four parts separated by blanks:
* Year Matcher Expression
* Month Matcher Expression
* Day Of Month Matcher Expression
* Day Of Week Matcher Expression

Each part can be a compination of multiple expression seprated by a comma.

#### Year Matcher Expression

A year matcher expression is made up by a range specification and, optionally, a year predicate.
Year predicates are appended to the range expression.

##### Year Range Expressions

|Expression     | Description
|---------------|--------------------------------
|*              | Any year
|2000..         | Any year from 2000 included
|..2000         | Any year until 2000 included  
|2000..2020     | Any year between 2000 and 2020 included

##### Year Predicate Expressions

|Expression     |Predicate|Description
|---------------|---------|-------------------------
|*%2            |Modulo   |Any year which modulo 2 is zero
|2000../2       |Periodic |Every 2 years starting from 2000. **ATTENTION**: this predicate requires that the range is left closed.
|*/Leap         |Leap     |Every leap year  
|*/NotLeap      |NotLeap  |Every not leap year

#### Month Matcher Expression

A month matcher expression is made up by a range specification and, optionally, a month predicate.
Month predicates are appended to the range expression.

Months are represented by the numbers 1 (January) to 12 (December).

##### Month Range Expressions

|Expression     | Description
|---------------|--------------------------------
|*              | Any month
|2..            | Any month from february included
|..11           | Any month until november included  
|2..11          | Any month between february and november

##### Month Predicate Expressions

|Expression     |Predicate|Description
|---------------|---------|-------------------------
|*%2            |Modulo   |Any month which modulo 2 is zero
|1../2          |Periodic |January, March, May, July, September, November. **ATTENTION**: this predicate requires that the range is left closed.

#### Day Of Month Matcher Expression

A day of month matcher expression is made up by a range specification and, optionally, a day of month predicate.
Day of month predicates are appended to the range expression.

Day of months are represented by the numbers 1 (the 1st) to 31 (the 31st).

##### Day Of Month Range Expressions

|Expression     | Description
|---------------|--------------------------------
|*              | Any day of month
|2..            | Any day of month from the 2nd included
|..11           | Any day of month until the 11th included  
|2..11          | Any day of month between the 2nd and the 11th

##### Day Of Month Predicate Expressions

|Expression     |Predicate|Description
|---------------|---------|-------------------------
|*%2            |Modulo   |Any day of month wich modulo 2 is zero
|1../2          |Periodic |The day of months that are odd. **ATTENTION**: this predicate requires that the range is left closed.

#### Day Of Week Matcher Expression

A day of week matcher expression is made up by a range specification and.

Day of weeks are represented by the numbers 0 (Sunday), 1 (Monday), … to 7 (Sunday again).

The double representation of Sunday simplifies writing rules for different cultural contexts.

##### Day Of Week Range Expressions

|Expression     | Description
|---------------|--------------------------------
|*              | Any day of week
|2..            | Any day of week from Tuesday to Sunday (7)
|..2            | Any day of week from Sunday (0) to Tuesday  
|1..5           | Any weekday
|6..7           | Any weekend

### Description Of The Day (Optional)

You can optionally provide a description of the day enclosing it in a pair of double brackets.

#### Example

The following day rule matches for every day.
```
* * * * [[every day]]
``` 

### Working Day Periods

You can optionally provide a comma separated list of working day periods.
A time period is specified by providing a begin and an end time.

A calendar rule without a working day period is interpreted as a non-working day rule.

**ATTENTION**: hours must be provided with two digits. 

```
00:00-24:00
``` 
 
 
 ## Calendar Examples
 
 For a working example please refer to the [Sample App](../master/Nexusat.Utils.CalendarGenerator.SampleApp/Main.cs).
 
 
 ### Typical working day calendar without holidays
 
```csharp
var rules = new CalendarRules
{
    {DayRulePolicy.Fallthrough, DayRuleParser.Parse("* * * * [[working day]] 08:30-13:30,14:30-17:30")},
    {DayRulePolicy.Fallthrough, DayRuleParser.Parse("* * * 6..7 [[non working day]]")}
};
var calendar = new Calendar("Standard working week", rules);
```

 ### Compensation day on 1st of March on Leap years
 
```csharp
var rules = new CalendarRules
{
    {DayRulePolicy.Fallthrough, DayRuleParser.Parse("*/Leap 3 1 * [[leap year compensation day]]")},
};
var calendar = new Calendar("Compensation day on leap years", rules);
```

## Calendar Table Days Generation

Once you have defined a Calendar you can generate a calendar day table.

The calendar days table generated by the [Sample App](../master/Nexusat.Utils.CalendarGenerator.SampleApp/Main.cs) are:

### Calendar Days Table of December 2020 (XML)

```xml
<?xml version="1.0" encoding="utf-16"?>
<CalendarDays>
	<Day date="2020-12-01" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-02" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-03" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-04" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-05" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-06" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-07" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-08" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-09" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-10" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-11" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-12" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-13" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-14" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-15" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-16" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-17" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-18" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-19" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-20" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-21" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-22" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-23" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-24" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-25" isWorkingDay="false">
		<Description>xmas</Description>
	</Day>
	<Day date="2020-12-26" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-27" isWorkingDay="false">
		<Description>non working day</Description>
	</Day>
	<Day date="2020-12-28" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-29" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-30" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
	<Day date="2020-12-31" isWorkingDay="true">
		<Description>working day</Description>
		<WorkingPeriods>
			<TimePeriod begin="08:30" end="13:30"/>
			<TimePeriod begin="14:30" end="17:30"/>
		</WorkingPeriods>
	</Day>
</CalendarDays>
```

### Calendar Days Table of December 2020 (JSON)
```json
[
  {
    "date": "2020-12-01",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-02",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-03",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-04",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-05",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-06",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-07",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-08",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-09",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-10",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-11",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-12",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-13",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-14",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-15",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-16",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-17",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-18",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-19",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-20",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-21",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-22",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-23",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-24",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-25",
    "description": "xmas",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-26",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-27",
    "description": "non working day",
    "isWorkingDay": false,
    "workingPeriods": null
  },
  {
    "date": "2020-12-28",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-29",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-30",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  },
  {
    "date": "2020-12-31",
    "description": "working day",
    "isWorkingDay": true,
    "workingPeriods": [
      {
        "begin": "08:30",
        "end": "13:30"
      },
      {
        "begin": "14:30",
        "end": "17:30"
      }
    ]
  }
]
```
