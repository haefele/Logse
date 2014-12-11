using System;
using Newtonsoft.Json.Linq;

namespace Xemio.Logse.Server.Data.Models
{
    public class CreateLogMessage
    {
        public Guid Session { get; set; }

        public DateTimeOffset ClientTimeStamp { get; set; }

        public LoggingLevel LogLevel { get; set; }
        public string Message { get; set; }

        public JToken AdditionalData { get; set; }
    }
}