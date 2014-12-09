using Castle.Core;
using Raven.Client;
using Xemio.Logse.Server.Data.Entities;

namespace Xemio.Logse.Server.Windsor.Startables
{
    public class GlobalPasswordStartable : IStartable
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalPasswordStartable"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public GlobalPasswordStartable(IDocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executed when this application starts.
        /// </summary>
        public void Start()
        {
            using (IDocumentSession session = this._documentStore.OpenSession())
            {
                var globalSettings = session.Load<GlobalSettings>(GlobalSettings.GlobalId);

                if (globalSettings != null)
                    return;

                globalSettings = new GlobalSettings();
                globalSettings.GlobalPassword.Change("Password");

                session.Store(globalSettings);

                session.SaveChanges();
            }
        }
        /// <summary>
        /// Executed when this application stops.
        /// </summary>
        public void Stop()
        {
        }
        #endregion
    }
}