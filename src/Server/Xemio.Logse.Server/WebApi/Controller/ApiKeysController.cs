using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Raven.Json.Linq;
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
    public class ApiKeysController : LogseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeysController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public ApiKeysController(IAsyncDocumentSession documentSession) 
            : base(documentSession)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("ApiKeys/Create")]
        public async Task<HttpResponseMessage> CreateApiKey([FromUri]int projectId, [FromUri]string name, [FromUri]ApiKeyMode mode)
        {
            string projectStringId = this.DocumentSession.Advanced.GetStringIdFor<Project>(projectId);
            var project = await this.DocumentSession.LoadAsync<Project>(projectStringId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var key = new ApiKey
            {
                ProjectId = project.Id,
                Key = Guid.NewGuid().ToString("N"),
                Name = name,
                Mode = mode
            };

            await this.DocumentSession.StoreAsync(key);

            return Request.CreateResponse(HttpStatusCode.Created, key.Key);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("ApiKeys")]
        public async Task<HttpResponseMessage> GetApiKeys([FromUri]int projectId)
        {
            string projectStringId = this.DocumentSession.Advanced.GetStringIdFor<Project>(projectId);
            var project = await this.DocumentSession.LoadAsync<Project>(projectStringId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var keys = await this.DocumentSession.Query<ApiKey, ApiKeys_ByProjectIdAndKey>()
                .Where(f => f.ProjectId == project.Id)
                .TransformWith<ApiKey_ToApiKeyModel, ApiKeyModel>()
                .ToListAsync();

            return Request.CreateResponse(HttpStatusCode.Found, keys);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("ApiKeys/{key}")]
        public async Task<HttpResponseMessage> GetApiKey([FromUri]string key)
        {
            string apiKeyStringId = this.DocumentSession.Advanced.GetStringIdFor<ApiKey>(key);

            ApiKeyModel theKey = await this.DocumentSession.LoadAsync<ApiKey_ToApiKeyModel, ApiKeyModel>(apiKeyStringId);

            if (theKey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.Found, theKey);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("ApiKeys/{key}/Activate")]
        public async Task<HttpResponseMessage> ActivateApiKey([FromUri]string key)
        {
            string apiKeyStringId = this.DocumentSession.Advanced.GetStringIdFor<ApiKey>(key);
            ApiKey apiKey = await this.DocumentSession.LoadAsync<ApiKey>(apiKeyStringId);

            if (apiKey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            apiKey.IsDeactivated = false;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("ApiKeys/{key}/Deactivate")]
        public async Task<HttpResponseMessage> DeactivateApiKey([FromUri]string key)
        {
            string apiKeyStringId = this.DocumentSession.Advanced.GetStringIdFor<ApiKey>(key);
            ApiKey apiKey = await this.DocumentSession.LoadAsync<ApiKey>(apiKeyStringId);

            if (apiKey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            apiKey.IsDeactivated = true;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}