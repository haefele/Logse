﻿using System;
using Newtonsoft.Json.Linq;

namespace Xemio.Logse.Server.Data.Entities
{
    internal class LogMessage : AggregateRoot
    {
        public Guid Session { get; set; }

        public DateTimeOffset ClientTimeStamp { get; set; }
        public DateTimeOffset ServerTimeStamp { get; set; }

        public LoggingLevel LogLevel { get; set; }
        public string Message { get; set; }

        public JToken AdditionalData { get; set; }

        public string ApiKey { get; set; }
        public string ProjectId { get; set; }
    }
}