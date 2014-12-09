using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;
using Xemio.Logse.Server.Extensions;
using Xemio.Logse.Server.Raven.Transformers;
using Xemio.Logse.Server.WebApi.Filters;

namespace Xemio.Logse.Server.WebApi.Controller
{
    public class ProjectsController : LogseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public ProjectsController(IAsyncDocumentSession documentSession) 
            : base(documentSession)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/Create")]
        public async Task<HttpResponseMessage> CreateProject([FromUri]string projectName)
        {
            var project = new Project
            {
                Name = projectName
            };

            await this.DocumentSession.StoreAsync(project);
            return Request.CreateResponse(HttpStatusCode.Created, project.Id.GetIntId());
        }

        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects")]
        public async Task<HttpResponseMessage> GetProjects()
        {
            IList<ProjectModel> projects = await this.DocumentSession
                .Query<Project>()
                .TransformWith<ProjectToProjectModel, ProjectModel>()
                .ToListAsync();

            return Request.CreateResponse(HttpStatusCode.Found, projects);
        }

        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}")]
        public async Task<HttpResponseMessage> GetProject(int projectId)
        {
            string projectStringId = this.DocumentSession.Advanced.GetStringIdFor<Project>(projectId);
            var project = await this.DocumentSession.LoadAsync<ProjectToProjectModel, ProjectModel>(projectStringId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.Found, project);
        }

        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}/Deactivate")]
        public async Task<HttpResponseMessage> DeactivateProject(int projectId)
        {
            var project = await this.DocumentSession.LoadAsync<Project>(projectId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            project.IsDeactivated = true;

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [RequiresGlobalPassword]
        [Route("Projects/{projectId:int}/Activate")]
        public async Task<HttpResponseMessage> ActivateProject(int projectId)
        {
            var project = await this.DocumentSession.LoadAsync<Project>(projectId);

            if (project == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            project.IsDeactivated = false;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}