using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Abstractions.Util;
using Raven.Client;
using Xemio.Logse.Server.Data.Models;
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
        [RequiresApiKey]
        [HttpGet]
        [Route("LogMessages")]
        public Task<HttpResponseMessage> GetLogMessages([FromUri] int projectId)
        {
            return new CompletedTask<HttpResponseMessage>(new HttpResponseMessage());
        }

        [RequiresApiKey]
        public Task<HttpResponseMessage> CreateLogMessages([FromUri]string apiKey, [FromBody]CreateLogMessage[] createLogMessages)
        {
            return new CompletedTask<HttpResponseMessage>(new HttpResponseMessage());
        }
        #endregion
    }
}