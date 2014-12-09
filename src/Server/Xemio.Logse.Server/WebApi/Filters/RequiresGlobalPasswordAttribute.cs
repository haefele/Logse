using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Extensions;
using Xemio.Logse.Server.Entities;
using Xemio.Logse.Server.Extensions;

namespace Xemio.Logse.Server.WebApi.Filters
{
    public class RequiresGlobalPasswordAttribute : AuthorizeAttribute
    {
        #region Methods
        /// <summary>
        /// Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, string.Empty);
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
                var globalSettings = documentSession.LoadAsync<GlobalSettings>(1).Result;
                if (globalSettings == null)
                {
                    globalSettings = new GlobalSettings();
                    globalSettings.GlobalPassword.Change("password");
                }

                string givenHash = this.ExtractHash(actionContext);

                return globalSettings.GlobalPassword.IsCorrect(givenHash);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Extracts the hash.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private string ExtractHash(HttpActionContext context)
        {
            if (context.Request.Headers.Authorization != null &&
                context.Request.Headers.Authorization.Scheme == "Logse")
            {

                return context.Request.Headers.Authorization.Parameter;
            }

            NameValueCollection query = context.Request.RequestUri.ParseQueryString();
            return query["GlobalPasswordHash"];
        }
        #endregion
    }
}