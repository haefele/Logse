using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Xemio.Logse.Server.Data.Entities;

namespace Xemio.Logse.Server.WebApi.Controller
{
    public abstract class LogseController : ApiController
    {
        #region Properties
        /// <summary>
        /// Gets the document store.
        /// </summary>
        public IDocumentStore DocumentStore
        {
            get { return this.DocumentSession.Advanced.DocumentStore; }
        }
        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IAsyncDocumentSession DocumentSession { get; private set; }
        /// <summary>
        /// Gets or sets the authenticated API key.
        /// </summary>
        internal ApiKey AuthenticatedApiKey { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LogseController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        protected LogseController(IAsyncDocumentSession documentSession)
        {
            this.DocumentSession = documentSession;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.ExecuteAsync(controllerContext, cancellationToken);

            using (this.DocumentSession)
            {
                await this.DocumentSession.SaveChangesAsync();
            }

            return response;
        }
        #endregion

    }
}