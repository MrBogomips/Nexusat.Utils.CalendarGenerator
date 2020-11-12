using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    public static class CalendarSerializer
    {
        private static int CalendarSerializerVersion = 1;
        
        [XmlRoot(ElementName = "Calendar", Namespace = "http://www.nexusat.it/schemas/calendar")]
        public class CalendarDocument
        {
            public class Period
            {
                [XmlAttribute(AttributeName = "begin")]
                public string Begin { get; set; }
                [XmlAttribute(AttributeName = "end")]
                public string End { get; set; }
            }
            public class Rule
            {
                [XmlAttribute(AttributeName = "policy")]
                public DayRulePolicy Policy { get; set; }
                [XmlAttribute(AttributeName = "description")]
                public string Description { get; set; }
                [XmlAttribute(AttributeName = "pattern")]
                public string Pattern { get; set; }
                public List<Period> WorkingPeriods { get; set; }
            }
            
            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            public string Description { get; set; }
            public string LongDescription { get; set; }
            public List<Rule> Rules { get; set; }

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
                Rules = calendar.CalendarRules.Select(_ => new Rule
                {
                    Policy = _.Policy,
                    Description = _.Rule.Description,
                    Pattern = _.Rule.ToDayPatternString(),
                    WorkingPeriods = _.Rule.WorkingPeriods?.Select(_ => new Period
                        {Begin = _.Begin.ToString(), End = _.End.ToString()}).ToList()
                }).ToList();
            }

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
                    {
                        throw new SerializationException($"Day pattern '{r.Pattern}' is invalid");
                    }
                    if (!string.IsNullOrEmpty(description))
                        throw new SerializationException($"Day pattern '{r.Pattern}' is invalid in this context: you cannot provide a day description");
                    if (workingPeriods is not null)
                        throw new SerializationException($"Day pattern '{r.Pattern}' is invalid in this context: you cannot provide a working period list");
                    rules.Add(r.Policy, r.Description, yearMatchers, monthMatchers, dayOfMonthMatchers, dayOfWeekMatchers, 
                        r.WorkingPeriods?.Select(_ => new CalendarGenerator.TimePeriod(Time.Parse(_.Begin), Time.Parse(_.End))));
                }
                return new Calendar(Name, rules, Description, LongDescription);
            }
        }
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
            var document = (CalendarDocument)ser.Deserialize(reader);
            Debug.Assert(document != null, "Calendar serialization returned null");
            return document.GetCalendar();
        }

        /// <summary>
        ///     Returns an UTF-8 encoded JSON representing the calendar
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string ToJson(this Calendar calendar, bool indent = false)
        {
            if (calendar == null) throw new ArgumentNullException(nameof(calendar));
            var document = new CalendarDocument(calendar);
            
            throw new NotImplementedException();
        }

        public static Calendar LoadFromJson(string json)
        {
            throw new NotImplementedException();
        }

        public static Calendar ReadFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public static void WriteToFile(this Calendar calendar, string filename)
        {
            throw new NotImplementedException();
        }

        public static Calendar ReadFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public static void WriteToStream(this Calendar calendar, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}