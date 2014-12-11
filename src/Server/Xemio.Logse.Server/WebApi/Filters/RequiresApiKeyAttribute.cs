using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Linq;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Extensions;
using Xemio.Logse.Server.Raven.Indexes;

namespace Xemio.Logse.Server.WebApi.Filters
{
    public class RequiresApiKeyAttribute : AuthorizeAttribute
    {
        #region Methods
        /// <summary>
        /// Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
        /// <summary>
        /// Indicates whether the specified control is authorized.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var documentSession = actionContext.ControllerContext.Configuration.DependencyResolver.GetService<IAsyncDocumentSession>();

            using (documentSession.Advanced.DocumentStore.AggressivelyCache())
            {
                string key = this.ExtractApiKey(actionContext);

                ApiKey apiKey = documentSession.Query<ApiKey, ApiKeys_ByProjectIdAndKey>()
                    .FirstOrDefaultAsync(f => f.Key == key)
                    .Result;

                if (apiKey == null)
                    return false;

                return apiKey.IsDeactivated == false;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Extracts the API key.
        /// </summary>
        /// <param name="context">The context.</param>
        private string ExtractApiKey(HttpActionContext context)
        {
            if (context.Request.Headers.Authorization != null &&
                context.Request.Headers.Authorization.Scheme == "Logse")
            {

                return context.Request.Headers.Authorization.Parameter;
            }

            NameValueCollection query = context.Request.RequestUri.ParseQueryString();
            return query["ApiKey"];
        }
        #endregion
    }
}