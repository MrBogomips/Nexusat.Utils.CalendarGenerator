using System;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    public interface IDateMatcher
    {
        bool Match(DateTime date);
    }
}