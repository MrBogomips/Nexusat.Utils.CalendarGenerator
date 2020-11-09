using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Nexusat.Utils.CalendarGenerator.CronDayRule
{
    
    public abstract class YearMatcher: IDateMatcher {
        private static Regex ParseRegEx { get; } = new Regex(@"^(\*|\d+)(/(Leap)|(NotLeap)|\d+)?$");
        /// <summary>
        /// Parse a year match expression.
        /// <example>
        /// Valid expression are:
        /// * => Every year
        /// *..* => Every year also
        /// 2020 => Exactly that year
        /// *..2030 => Every year until 2030 comprised
        /// 2020..* => Every year starting from 2020
        /// 2020-2030 => Every year between 2020 and 2030
        /// */Leap => Every leap year
        /// 2020-2030/Leap => Every leap year between 2020 and 2030
        /// */NonLeap => Every non leap year
        /// 2020..2030/2 => Starting from 2020 every 2 years until 2030
        /// 2020/2 => Starting from 2020 every 2 years
        /// 2020/NonLeap => Starting from 2020 every NonLeap year
        /// 2020..2030%2 => Every year which reminder modulo 2 is zero between 2020 and 2030
        /// 2020%3 => Starting from 2020 every year which reminder modulo 3 is zero
        /// </example>
        /// </summary>
        public static RangeYearMatcher Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            
            throw new NotImplementedException();
        }

        public abstract bool Match(DateTime date);
    }
    
    public class RangeYearMatcher: YearMatcher
    {
        public int? FirstYear { get; }
        public int? LastYear { get; }
        public override bool Match(DateTime date) => (!FirstYear.HasValue || FirstYear.Value <= date.Year)
                                                     &&
                                                     (!LastYear.HasValue || LastYear.Value >= date.Year);
        

        public RangeYearMatcher(int? firstYear, int? lastYear)
        {
            if (firstYear is not null && firstYear.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(firstYear), "Must be a positive value");
            }
            if (lastYear is not null && lastYear < firstYear)
            {
                throw new ArgumentOutOfRangeException(nameof(lastYear), $"Must be greater than or equal to '{nameof(firstYear)}'");
            }
            FirstYear = firstYear;
            LastYear = lastYear;
        }

        public bool IsLeftOpenRange => !FirstYear.HasValue;
        public bool IsRightOpenRange => !LastYear.HasValue;
        public bool IsOpenRange => !FirstYear.HasValue || !LastYear.HasValue;
        public bool IsLeftRightOpenRange => !FirstYear.HasValue && !LastYear.HasValue;
        public bool IsClosedRange => FirstYear.HasValue && LastYear.HasValue;
        public bool IsOneYear => FirstYear.HasValue && LastYear.HasValue && FirstYear.Value == LastYear.Value;

        public override string ToString() => (FirstYear, LastYear) switch
            {
                (null, null) => "*",
                (var fy, null) => $"{fy}..",
                (null, var ly) => $"..{ly}",
                var (fy, ly) => fy == ly ? fy.Value.ToString() : $"{fy}..{ly}"
            };
    }
    
    public class ModuloYearMatcher : RangeYearMatcher
    {
        public int Modulo { get; }
        public ModuloYearMatcher(int? firstYear, int? lastYear, int modulo): base(firstYear, lastYear)
        {
            if (modulo < 2) throw new ArgumentException($"'{nameof(modulo)}' must be greater than 1");
            if (IsOneYear) throw new ArgumentException("You must specify a multi year range");
            Modulo = modulo;
        }
        
        public override bool Match(DateTime date) => base.Match(date) && date.Year % Modulo == 0;
    }

    public class PeriodicYearMatcher : RangeYearMatcher
    {
        public int Period { get; }
        
        public PeriodicYearMatcher(int firstYear, int? lastYear, int period): base(firstYear, lastYear)
        {
            if (period < 2) throw new ArgumentException($"'{nameof(period)}' must be greater than 1");
            if (IsOneYear) throw new ArgumentException("You must specify a multi year range");
            Period = period;
        }

        public override bool Match(DateTime date) => base.Match(date) && (date.Year - FirstYear.Value) % Period == 0;

        public override string ToString() => $"{base.ToString()}/{Period}";
    }
    public class LeapYearMatcher: RangeYearMatcher
    {
        public LeapYearMatcher(int? firstYear, int? lastYear) : base(firstYear, lastYear)
        {
        }
        
         internal static bool IsLeapYear(DateTime date) 
             => date.Year % 4 == 0 && (date.Year % 100 != 0 || date.Year % 400 == 0);

         public override bool Match(DateTime date) => base.Match(date) && IsLeapYear(date);
         
         public override string ToString() => $"{base.ToString()}/Leap";
    }

    public class NonLeapYearMatcher : RangeYearMatcher
    {
        public NonLeapYearMatcher(int? firstYear, int? lastYear) : base(firstYear, lastYear)
        {
            
        }
        
        public override bool Match(DateTime date) => base.Match(date) && !LeapYearMatcher.IsLeapYear(date);
        public override string ToString() => $"{base.ToString()}/NotLeap";
    }
    
    
}