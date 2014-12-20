using System.Linq;
using Raven.Client.Indexes;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;

namespace Xemio.Logse.Server.Raven.Transformers
{
    internal class LogMessage_ToLogMessageModel : AbstractTransformerCreationTask<LogMessage>
    {
        public LogMessage_ToLogMessageModel()
        {
            this.TransformResults = logMessages =>
                from message in logMessages
                select new LogMessageModel
                {
                    Id = int.Parse(message.Id.Split('/').Last()),
                   
                    Session = message.Session,

                    ClientTimeStamp = message.ClientTimeStamp,
                    ServerTimeStamp = message.ServerTimeStamp,

                    LogLevel = message.LogLevel,
                    Message = message.Message,

                    AdditionalData = message.AdditionalData,
                };
        }
    }
}