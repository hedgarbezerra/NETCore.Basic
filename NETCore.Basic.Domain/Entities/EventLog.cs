using System;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace NETCore.Basic.Domain.Entities
{
    public enum LogLevel
    {
        //
        // Resumo:
        //     Anything and everything you might want to know about a running block of code.
        Verbose = 0,
        //
        // Resumo:
        //     Internal system events that aren't necessarily observable from the outside.
        Debug = 1,
        //
        // Resumo:
        //     The lifeblood of operational intelligence - things happen.
        Information = 2,
        //
        // Resumo:
        //     Service is degraded or endangered.
        Warning = 3,
        //
        // Resumo:
        //     Functionality is unavailable, invariants are broken or data is lost.
        Error = 4,
        //
        // Resumo:
        //     If you have a pager, it goes off when one of these occurs.
        Fatal = 5
    }
    public class EventLog
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        [JsonIgnore]
        public XElement XmlContent
    {
            get { return XElement.Parse(Properties); }
            set { Properties = value.ToString(); }
        }
        public DateTime CreatedTime { get; set; }
        public EventLog()
        {

        }

        public EventLog(string message, string exception, LogLevel logLevel)
        {
            Message = message;
            Exception = exception;
            LogLevel = logLevel;
        }
        public EventLog(string message, string messageTemplate, string exception, string properties, LogLevel logLevel)
        {
            Message = message;
            MessageTemplate = messageTemplate;
            Exception = exception;
            Properties = properties;
            LogLevel = logLevel;
        }
    }

}
