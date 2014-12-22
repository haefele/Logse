using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using Raven.Database.Config;
using Raven.Server;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Raven.Indexes;

namespace Xemio.Logse.Server.Windsor
{
    public class CommonWindsorInstaller : IWindsorInstaller
    {
        #region Methods
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //WebAPI Controller
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<ApiController>()
                .WithServiceSelf()
                .LifestyleScoped());

            //RavenDB
            RavenDbServer server = this.CreateRavenDbServer();

            container.Register(
                Component.For<IDocumentStore>().Instance(server.DocumentStore).LifestyleSingleton(),
                Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession()).LifestyleScoped());

            //Startables
            container.AddFacility<StartableFacility>(f => f.DeferredStart());

            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IStartable>());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the raven database server.
        /// </summary>
        private RavenDbServer CreateRavenDbServer()
        {
            var config = new RavenConfiguration
            {
                Port = int.Parse(Dependency.OnAppSettingsValue("Logse/RavenHttpServerPort").Value),
                DataDirectory = Path.Combine(".", "Database", "Data"),
                PluginsDirectory = Path.Combine(".", "Database", "Plugins"),
                AssembliesDirectory = Path.Combine(".", "Database", "Assemblies"),
                EmbeddedFilesDirectory = Path.Combine(".", "Database", "Embedded"),
                Settings =
                {
                    { "Raven/CompiledIndexCacheDirectory", Path.Combine(".", "Database", "Raven") }
                }
            };

            var ravenDbServer = new RavenDbServer(config);
            this.CustomizeDocumentStore(ravenDbServer.DocumentStore);
            ravenDbServer.Initialize();

            ravenDbServer.DocumentStore.DefaultDatabase = Dependency.OnAppSettingsValue("Logse/RavenName").Value;
            ravenDbServer.DocumentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(ravenDbServer.DocumentStore.DefaultDatabase);

            IndexCreation.CreateIndexes(this.GetType().Assembly, ravenDbServer.DocumentStore);

            if (bool.Parse(Dependency.OnAppSettingsValue("Logse/EnableRavenHttpServer").Value))
            {
                ravenDbServer.EnableHttpServer();
            }

            return ravenDbServer;
        }
        /// <summary>
        /// Customizes the document store.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        private void CustomizeDocumentStore(DocumentStore documentStore)
        {
            documentStore.Conventions.RegisterIdConvention<ApiKey>((dbName, databaseCommands, entity) =>
            {
                string typeTagName = documentStore.Conventions.GetTypeTagName(entity.GetType());
                return string.Format("{0}/{1}", typeTagName, entity.Key);
            });
            documentStore.Conventions.RegisterAsyncIdConvention<ApiKey>((dbName, databaseCommands, entity) =>
            {
                string typeTagName = documentStore.Conventions.GetTypeTagName(entity.GetType());
                return Task.FromResult(string.Format("{0}/{1}", typeTagName, entity.Key));
            });
        }
        #endregion
    }
}