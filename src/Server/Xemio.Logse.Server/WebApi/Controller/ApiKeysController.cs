using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Raven.Json.Linq;
using Xemio.Logse.Server.Data;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;
using Xemio.Logse.Server.Extensions;
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
        [Route("Projects/{projectId:int}/ApiKeys/Create")]
        public async Task<HttpResponseMessage> CreateApiKey(int projectId, string name, ApiKeyMode mode)
        {
            var project = await this.DocumentSession.LoadAsync<Project>(projectId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var key = new ApiKey
            {
                Key = Guid.NewGuid().ToString("N"),
                Name = name,
                Mode = mode
            };
            project.ApiKeys.Add(key);

            return Request.CreateResponse(HttpStatusCode.Created, key.Key);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}/ApiKeys")]
        public async Task<HttpResponseMessage> GetApiKeys(int projectId)
        {
            var project = await this.DocumentSession.LoadAsync<Project>(projectId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            string projectStringId = this.DocumentSession.Advanced.GetStringIdFor<Project>(projectId);
            ApiKeyModel[] keys = await this.DocumentSession.LoadAsync<ProjectToApiKeyModels, ApiKeyModel[]>(projectStringId);

            return Request.CreateResponse(HttpStatusCode.Found, keys);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}/ApiKeys/{key}")]
        public async Task<HttpResponseMessage> GetApiKey(int projectId, string key)
        {
            string projectStringId = this.DocumentSession.Advanced.GetStringIdFor<Project>(projectId);
            ApiKeyModel theKey = await this.DocumentSession.LoadAsync<ProjectToApiKeyModels, ApiKeyModel>(projectStringId, f => f.AddTransformerParameter("key", RavenJToken.FromObject(key)));

            if (theKey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.Found, theKey);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}/ApiKeys/{key}/Activate")]
        public async Task<HttpResponseMessage> ActivateApiKey(int projectId, string key)
        {
            Project project = await this.DocumentSession.LoadAsync<Project>(projectId);
            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var apiKey = project.ApiKeys.FirstOrDefault(f => f.Key == key);
            if (apiKey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            apiKey.IsDeactivated = false;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}/ApiKeys/{key}/Deactivate")]
        public async Task<HttpResponseMessage> DeactivateApiKey(int projectId, string key)
        {
            Project project = await this.DocumentSession.LoadAsync<Project>(projectId);
            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var apiKey = project.ApiKeys.FirstOrDefault(f => f.Key == key);
            if (apiKey == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            apiKey.IsDeactivated = true;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}