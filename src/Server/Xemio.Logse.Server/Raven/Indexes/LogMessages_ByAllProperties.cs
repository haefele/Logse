using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Database.Indexing.Collation.Cultures;
using Xemio.Logse.Server.Data.Entities;

namespace Xemio.Logse.Server.Raven.Indexes
{
    // ReSharper disable once InconsistentNaming
    internal class LogMessages_ByAllProperties : AbstractIndexCreationTask<LogMessage>
    {
        public LogMessages_ByAllProperties()
        {
            this.Map = messages =>
                from message in messages
                select new
                {
                    message.Session,

                    message.ClientTimeStamp,
                    message.ServerTimeStamp,

                    message.LogLevel,
                    message.Message,

                    message.ProjectId,
                };

            this.Index(f => f.Message, FieldIndexing.Analyzed);
        }
    }
}