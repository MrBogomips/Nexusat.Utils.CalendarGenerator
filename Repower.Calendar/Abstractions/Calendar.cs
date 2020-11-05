using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Repower.Calendar
{
    // TODO: Json Serializer: Json serializer
    // TODO: Calendar Generator: generate a calendar suitable to be represented
    // TODO: EasterHolidayRule: manage the easter
    // TODO: CustomDayRule: a way to define a rule for specific days
    // TODO: XSD Exporter for all the Rules settings

    /// <summary>
    /// Base implementation of a calendar.
    /// 
    /// See <see cref="ICalendar"/> for the semantic of the members.
    /// </summary>
    [DataContract(Namespace ="http://www.nexusat.it/schemas/calendar")]
    public class Calendar : ICalendar
    {
        [DataMember]
        private readonly DayRules DayRules;

        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public string LongDescription { get; private set; }

        public Calendar(string name, DayRules dayRules, string description = null, string longDescription = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DayRules = dayRules ?? throw new ArgumentNullException(nameof(dayRules));
        }

        /// <summary>
        /// Return the working day info of the day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns><c>null</c> if the calendar has no workingDayInfo</returns>
        public IDayInfo GetDayInfo(DateTime date)
        {
            IDayInfo dayInfo;
            TryGetDayInfo(date, out dayInfo);
            return dayInfo;
        }
        public bool TryGetDayInfo(DateTime date, out IDayInfo dayInfo)
        {
            dayInfo = null;
            if (DayRules == null || !DayRules.Any())
            {
                return false; // no rules => nothing to do
            }
            else
            {
                foreach (var rule in DayRules)
                {
                    IDayInfo curInfo;

                    if (rule.Rule.TryGetDayInfo(date, out curInfo))
                    { // something to evaluate
                        dayInfo = curInfo; // register the most recent found
                        if (rule.Policy == DayRulePolicy.AcceptAlways ||
                            dayInfo.IsWorkingDay && rule.Policy == DayRulePolicy.AcceptIfTrue ||
                            !dayInfo.IsWorkingDay && rule.Policy == DayRulePolicy.AcceptIfFalse)
                        {
                            break;
                        }
                    }
                }
            }
            return dayInfo != null;
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
            {
                throw new ArgumentException($"'{nameof(xml)}' cannot be null or whitespace", nameof(xml));
            }

            var ser = new DataContractSerializer(typeof(Calendar));

            using var buffer = new StringReader(xml);
            using var reader = XmlReader.Create(buffer);
            return ser.ReadObject(reader) as Calendar;
        }

        /*
    public static Calendar LoadFromXml(string xml)
    {
        if (xml == null) throw new ArgumentNullException(nameof(xml));
        XmlSerializer ser = new XmlSerializer(typeof(Calendar));
        using (StringReader reader = new StringReader(xml))
        {
            var serializationInfo = ser.Deserialize(reader) as Calendar;
            return GetCalendarFromSerializationInfo(serializationInfo);
        }
    }
        */

        #region Equals
        // override object.Equals
        public bool Equals(Calendar that) => that != null && that.Name == this.Name;

        public override bool Equals(object that) => Equals(that as Calendar);

        // override object.GetHashCode
        public override int GetHashCode() => Name.GetHashCode();
        #endregion Equals
    }
}
