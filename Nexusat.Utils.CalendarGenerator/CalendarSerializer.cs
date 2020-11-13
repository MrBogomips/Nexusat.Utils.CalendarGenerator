using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Nexusat.Utils.CalendarGenerator
{
    public static class CalendarSerializer
    {
        private static readonly int CalendarSerializerVersion = 1;

        public static string ToXml(this Calendar calendar, XmlWriterSettings settings = null)
        {
            if (calendar == null) throw new ArgumentNullException(nameof(calendar));
            var document = new CalendarDocument(calendar);

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.nexusat.it/schemas/calendar");
            var ser = new XmlSerializer(typeof(CalendarDocument));
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(sb, settings);
            ser.Serialize(writer, document, ns);
            writer.Flush();
            return sb.ToString();
        }

        public static Calendar LoadFromXml(string xml)
        {
            if (xml == null) throw new ArgumentNullException(nameof(xml));
            var ser = new XmlSerializer(typeof(CalendarDocument));
            using var reader = new StringReader(xml);
            var document = (CalendarDocument) ser.Deserialize(reader);
            Debug.Assert(document != null, "Calendar serialization returned null");
            return document.GetCalendar();
        }

        /// <summary>
        ///     Returns an UTF-8 encoded JSON representing the calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string ToJson(this Calendar calendar, bool indent = false)
        {
            if (calendar == null) throw new ArgumentNullException(nameof(calendar));
            var document = new CalendarDocument(calendar);

            var ser = new DataContractJsonSerializer(typeof(CalendarDocument));

            using var buffer = new MemoryStream();
            using var writer = JsonReaderWriterFactory.CreateJsonWriter(buffer, Encoding.UTF8, true, indent);
            using var reader = new StreamReader(buffer);

            ser.WriteObject(writer, document);
            writer.Flush();
            buffer.Position = 0;
            return reader.ReadToEnd();
        }

        public static Calendar LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException($"'{nameof(json)}' cannot be null or whitespace", nameof(json));

            var ser = new DataContractJsonSerializer(typeof(CalendarDocument));

            using var buffer = new MemoryStream();
            using var writer = new StreamWriter(buffer);
            writer.Write(json);
            writer.Flush();
            buffer.Position = 0;
            var document = ser.ReadObject(buffer) as CalendarDocument;
            Debug.Assert(document != null, "Calendar serialization returned null");
            return document.GetCalendar();
        }

        [XmlRoot(ElementName = "Calendar", Namespace = "http://www.nexusat.it/schemas/calendar")]
        public class CalendarDocument
        {
            // ReSharper disable once UnusedMember.Global
            public CalendarDocument()
            {
            }

            public CalendarDocument(Calendar calendar)
            {
                if (calendar == null) throw new ArgumentNullException(nameof(calendar));
                Version = CalendarSerializerVersion.ToString();
                Name = calendar.Name;
                Description = calendar.Description;
                LongDescription = calendar.LongDescription;
                Rules = calendar.CalendarRules.Select(cr => new Rule
                {
                    Policy = cr.Policy.ToString(),
                    Description = cr.Rule.Description,
                    Pattern = cr.Rule.ToDayPatternString(),
                    WorkingPeriods = cr.Rule.WorkingPeriods?.Select(wp => new Period
                        {Begin = wp.Begin.ToString(), End = wp.End.ToString()}).ToList()
                }).ToList();
            }

            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }

            [XmlAttribute(AttributeName = "name")] public string Name { get; set; }

            public string Description { get; set; }
            public string LongDescription { get; set; }
            public List<Rule> Rules { get; set; }

            internal Calendar GetCalendar()
            {
                var rules = new CalendarRules();
                foreach (var r in Rules)
                {
                    if (!DayRuleParser.TryParseInternal(r.Pattern,
                        out var description,
                        out var yearMatchers,
                        out var monthMatchers,
                        out var dayOfMonthMatchers,
                        out var dayOfWeekMatchers,
                        out var workingPeriods))
                        throw new SerializationException($"Day pattern '{r.Pattern}' is invalid");
                    if (!string.IsNullOrEmpty(description))
                        throw new SerializationException(
                            $"Day pattern '{r.Pattern}' is invalid in this context: you cannot provide a day description");
                    if (workingPeriods is not null)
                        throw new SerializationException(
                            $"Day pattern '{r.Pattern}' is invalid in this context: you cannot provide a working period list");
                    rules.Add(Enum.Parse<DayRulePolicy>(r.Policy), r.Description, yearMatchers, monthMatchers,
                        dayOfMonthMatchers, dayOfWeekMatchers,
                        r.WorkingPeriods?.Select(_ => new TimePeriod(Time.Parse(_.Begin), Time.Parse(_.End))));
                }

                return new Calendar(Name, rules, Description, LongDescription);
            }

            public class Period
            {
                [XmlAttribute(AttributeName = "begin")]
                public string Begin { get; set; }

                [XmlAttribute(AttributeName = "end")] public string End { get; set; }
            }

            public class Rule
            {
                [XmlAttribute(AttributeName = "policy")]
                public string Policy { get; set; }

                [XmlAttribute(AttributeName = "description")]
                public string Description { get; set; }

                [XmlAttribute(AttributeName = "pattern")]
                public string Pattern { get; set; }

                public List<Period> WorkingPeriods { get; set; }
            }
        }
    }
}