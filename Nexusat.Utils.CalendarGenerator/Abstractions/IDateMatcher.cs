using System;

namespace Nexusat.Utils.CalendarGenerator
{
    public interface IDateMatcher
    {
        bool Match(DateTime date);
    }
}