using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Linq;
using Xemio.Logse.Server.Data;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;
using Xemio.Logse.Server.Extensions;
using Xemio.Logse.Server.Raven.Indexes;
using Xemio.Logse.Server.Raven.Transformers;
using Xemio.Logse.Server.WebApi.Filters;

namespace Xemio.Logse.Server.WebApi.Controller
{
    public class LogMessagesController : LogseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessagesController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public LogMessagesController(IAsyncDocumentSession documentSession) 
            : base(documentSession)
        {
        }
        #endregion

        #region Methods
        [RequiresApiKey(ApiKeyMode.Read)]
        [HttpGet]
        [Route("LogMessages")]
        public async Task<HttpResponseMessage> GetLogMessages([FromUri] int projectId, [FromUri]int skip = 0, [FromUri]int take = 20)
        {
            string projectStringId = this.DocumentSession.Advanced.GetStringIdFor<Project>(projectId);

            IList<LogMessageModel> logMessages = await this.DocumentSession.Query<LogMessage, LogMessages_ByAllProperties>()
                .Where(f => f.ProjectId == projectStringId)
                .OrderByDescending(f => f.ClientTimeStamp)
                .TransformWith<LogMessage_ToLogMessageModel, LogMessageModel>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Request.CreateResponse(HttpStatusCode.Found, logMessages);
        }

        [RequiresApiKey(ApiKeyMode.Write)]
        [HttpPost]
        [Route("LogMessages/Bulk")]
        public async Task<HttpResponseMessage> CreateLogMessages([FromUri]string apiKey, [FromBody]CreateLogMessage[] createLogMessages)
        {
            foreach (var message in createLogMessages)
            {
                await this.DocumentSession.StoreAsync(new LogMessage
                {
                    Session = message.Session,
                    ClientTimeStamp = message.TimeStamp,
                    ServerTimeStamp = DateTimeOffset.Now,
                    LogLevel = message.LogLevel,
                    Message = message.Message,
                    AdditionalData = message.AdditionalData,
                    ApiKey = this.AuthenticatedApiKey.Id,
                    ProjectId = this.AuthenticatedApiKey.ProjectId
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}