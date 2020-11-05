using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Repower.Calendar.Abstractions
{
    /// <summary>
    /// Represents a dump of a calendar period
    /// </summary>
    [XmlRoot(ElementName ="CalendarDays")]
    public sealed class CalendarDays: List<CalendarDays.Day>
    {
        public class TimePeriod
        {
            [XmlAttribute(AttributeName = "begin")]
            public string Begin { get; set; }
            [XmlAttribute(AttributeName = "end")]
            public string End { get; set; }
        }

        public class Day
        {
           
            [XmlAttribute(AttributeName = "date")]
            public string Date { get; set; }
            [XmlElement]
            public string Description { get; set; }
            [XmlAttribute(AttributeName = "isWorkingDay")]
            public bool IsWorkingDay { get; set; }
            public List<TimePeriod> WorkingPeriods { get; set; }
        }

        public string ToXml(XmlWriterSettings settings = null)
        {
            /* XML Layout
            <CalendarDays>
	            <Day date="ISO_DATE" isWorkingDay="true|false">
		            <Description></Description>
		            <WorkingPeriods>
			            <TimePeriod begin="08:00" end="12:00" />
			            <TimePeriod begin="14:00" end="18:00" />
		            </WorkingPeriods>
	            </Day>
            <CalendarDays>
            */
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(CalendarDays));
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(sb, settings);
            ser.Serialize(writer, this, ns);
            writer.Flush();
            return sb.ToString();
        }

        public string ToJson()
        {
            /* TODO: JSON Layout
            {
	            "days": [{
		            "date": "ISO_DATE",
		            "isWorkingDay": true | false,
		            "description": "",
		            "workingPeriods": [{
			            "begin": "08:00",
			            "end": "12:00"
		            },{
			            "begin": "14:00",
			            "end": "18:00"
		            },
		            ]
	            }]
	            }
            }
            */
            throw new NotImplementedException();
        }
    }
    
}
