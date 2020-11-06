# Nexusat.Utils.CalendarGenerator

.NET Library to generate a perpetual calendar based on rules.

For more information and detailed documentation please refer to the NexusAT Confluence space associated.

## Calendar Rules
A calendar implements a <em>chain of responsibility</em> pattern made up of <em>DayRules</em> that are applied in the order
they have been defined and in accordance with the responsibility policy.

### WeekdayWorkingDays
Rule to define working days based on week day names.

### WeekdayNonWorkingDays
Rule to define non-working days based on week day names.