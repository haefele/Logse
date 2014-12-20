using System;
using Newtonsoft.Json.Linq;

namespace Xemio.Logse.Server.Data.Models
{
    public class LogMessageModel
    {
        public int Id { get; set; }

        public Guid Session { get; set; }

        public DateTimeOffset ClientTimeStamp { get; set; }
        public DateTimeOffset ServerTimeStamp { get; set; }

        public LoggingLevel LogLevel { get; set; }
        public string Message { get; set; }

        public JToken AdditionalData { get; set; }
    }
}