using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation

namespace Nexusat.Utils.CalendarGenerator
{
    // TODO: CustomDayRule: a way to define a rule for specific days
    // TODO: CronDayRule: a way to define recurrent events
    // TODO: NuGet Packaging

    /// <summary>
    ///     Base implementation of a calendar.
    ///     See <see cref="ICalendar" /> for the semantic of the members.
    /// </summary>
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class Calendar : ICalendar, IEquatable<Calendar>
    {
        public static Calendar EmptyCalendar { get; } = new Calendar("EmptyCalendar", new DayRules());

        public Calendar(string name, DayRules dayRules, string description = null, string longDescription = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            LongDescription = longDescription;
            DayRules = dayRules ?? throw new ArgumentNullException(nameof(dayRules));
        }
        
        [field: DataMember(Name=nameof(DayRules))] public DayRules DayRules { get; }

        [field: DataMember(IsRequired = true, Name=nameof(Name))] public string Name { get; }

        [field: DataMember(Name=nameof(Description))] public string Description { get; }

        [field: DataMember(Name=nameof(LongDescription))] public string LongDescription { get; }

        /// <summary>
        ///     Return the working day info of the day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns><c>null</c> if the calendar has no workingDayInfo</returns>
        public DayInfo GetDayInfo(DateTime date)
        {
            TryGetDayInfo(date, out var dayInfo);
            return dayInfo;
        }

        public bool TryGetDayInfo(DateTime date, out DayInfo dayInfo)
        {
            dayInfo = null;
            if (DayRules == null || !DayRules.Any()) return false; // no rules => nothing to do

            foreach (var rule in DayRules)
            {
                if (!rule.Rule.TryGetDayInfo(date, out var curInfo)) continue; // something to evaluate
                dayInfo = curInfo; // register the most recent found
                if (rule.Policy == DayRulePolicy.AcceptAlways ||
                    dayInfo.IsWorkingDay && rule.Policy == DayRulePolicy.AcceptIfTrue ||
                    !dayInfo.IsWorkingDay && rule.Policy == DayRulePolicy.AcceptIfFalse)
                    break;
            }

            return dayInfo != null;
        }

        /// <summary>
        ///     Generate a <see cref="CalendarDays" /> info.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="defaultDayInfo">Default info in case of missing info</param>
        /// <returns></returns>
        public CalendarDays GenerateCalendarDays(DateTime from, DateTime to, DayInfo defaultDayInfo)
        {
            if (to < from) throw new ArgumentException($"'{nameof(to)}' cannot precede '{nameof(@from)}'");

            if (defaultDayInfo is null) throw new ArgumentNullException(nameof(defaultDayInfo));

            var calendarDays = new CalendarDays();
            for (var cur = from; cur <= to; cur = cur.AddDays(1))
            {
                var info = GetDayInfo(cur) ?? defaultDayInfo;
                var workingPeriods =
                    info.WorkingPeriods?.Select(wp => new CalendarDays.TimePeriod
                    {
                        Begin = wp.Begin.ToString(),
                        End = wp.End.ToString()
                    }).ToList();

                calendarDays.Add(new CalendarDays.Day
                {
                    Date = cur.ToString("yyyy-MM-dd"),
                    IsWorkingDay = info.IsWorkingDay,
                    Description = info.Description,
                    WorkingPeriods = workingPeriods
                });
            }

            return calendarDays;
        }

        public string ToXml(XmlWriterSettings settings = null)
        {
            var ser = new DataContractSerializer(typeof(Calendar));

            using var buffer = new StringWriter();
            using var writer = XmlWriter.Create(buffer, settings);

            ser.WriteObject(writer, this);
            writer.Flush();
            return buffer.ToString();
        }

        public static Calendar LoadFromXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentException($"'{nameof(xml)}' cannot be null or whitespace", nameof(xml));

            var ser = new DataContractSerializer(typeof(Calendar));

            using var buffer = new StringReader(xml);
            using var reader = XmlReader.Create(buffer);
            return ser.ReadObject(reader) as Calendar;
        }

        /// <summary>
        ///     Returns an UTF-8 encoded JSON representing the calendar
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToJson(bool indent = false)
        {
            var ser = new DataContractJsonSerializer(typeof(Calendar));

            using var buffer = new MemoryStream();
            using var writer = JsonReaderWriterFactory.CreateJsonWriter(buffer, Encoding.UTF8, true, indent);
            using var reader = new StreamReader(buffer);

            ser.WriteObject(writer, this);
            writer.Flush();
            buffer.Position = 0;
            return reader.ReadToEnd();
        }

        public static Calendar LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException($"'{nameof(json)}' cannot be null or whitespace", nameof(json));

            var ser = new DataContractJsonSerializer(typeof(Calendar));

            using var buffer = new MemoryStream();
            using var writer = new StreamWriter(buffer);
            writer.Write(json);
            writer.Flush();
            buffer.Position = 0;
            return ser.ReadObject(buffer) as Calendar;
        }

        /// <summary>
        ///     Add rules to the collection of rules
        /// </summary>
        /// <param name="rules"></param>
        public void AddRules(DayRules rules)
        {
            DayRules.AddRange(rules);
        }

        /// <summary>
        ///     Add the rules from the calendar to the collection of rules
        /// </summary>
        /// <param name="calendar"></param>
        public void AddRules(Calendar calendar)
        {
            DayRules.Add(calendar);
        }


        #region Equals

        // override object.Equals
        public bool Equals(Calendar other) => other is not null && Name == other.Name;

        public override bool Equals(object obj) => obj is Calendar c && Equals(c);

        // override object.GetHashCode
        public override int GetHashCode() => Name.GetHashCode();

        #endregion Equals
    }
}