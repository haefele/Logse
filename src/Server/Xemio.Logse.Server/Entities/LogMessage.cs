using System;
using Newtonsoft.Json.Linq;

namespace Xemio.Logse.Server.Entities
{
    internal class LogMessage : AggregateRoot
    {
        public Guid Session { get; set; }

        public DateTimeOffset ClientTimeStamp { get; set; }
        public DateTimeOffset ServerTimeStamp { get; set; }

        public LoggingLevel LogLevel { get; set; }
        public string Message { get; set; }

        public JObject AdditionalData { get; set; }

        public string ApiKeyId { get; set; }
    }
}