# Nexusat.Utils.CalendarGenerator

.NET Library to generate a perpetual calendar based on rules.

For more information and detailed documentation please refer to the NexusAT Confluence space associated.

## Calendar Definition

The definition of a calendar is obtained by specifing a set of rules.

Each rule is made up of the following parts:
* An evaluation policy
* A date matcher expression
* A description (optional)
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
|2000..2020     | Any year between 2000 and 2020

##### Year Predicate Expressions
|Expression     |Name    |Description
|---------------|--------|-------------------------
|*%2            |Modulo  |Any year which modulo 2 is zero
|2000../2       |Periodic|Every 2 years starting from 2000. **ATTENTION**: this predicate requires that the range is left closed.
|*/Leap         |Leap    |Every leap year  
|*/NotLeap      |NotLeap |Every not leap year

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
|Expression     |Name    |Description
|---------------|--------|-------------------------
|*%2            |Modulo  |Any month which modulo 2 is zero
|1../2          |Periodic|January, March, May, July, September, November. **ATTENTION**: this predicate requires that the range is left closed.

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
|Expression     |Name    |Description
|---------------|--------|-------------------------
|*%2            |Modulo  |Any day of month wich modulo 2 is zero
|1../2          |Periodic|The day of months that are odd

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

