using System;
using System.IO;
using System.Xml;

namespace Nexusat.Utils.CalendarGenerator
{
    public class CalendarSerializer
    {
        public string ToXml(XmlWriterSettings settings = null)
        {
            throw new NotImplementedException();
        }

        public Calendar LoadFromXml(string xml)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns an UTF-8 encoded JSON representing the calendar
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToJson(bool indent = false)
        {
            throw new NotImplementedException();
        }

        public Calendar LoadFromJson(string json)
        {
            throw new NotImplementedException();
        }

        public Calendar ReadFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void WriteToFile(string filename)
        {
            throw new NotImplementedException();
        }

        public Calendar ReadFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}