using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using Castle.Core;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using Raven.Database.Config;
using Raven.Server;

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
                Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession()));

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
                CompiledIndexCacheDirectory = Path.Combine(".", "Database", "Raven"),
                PluginsDirectory = Path.Combine(".", "Database", "Plugins"),
            };
            config.Settings.Add("Raven/CompiledIndexCacheDirectory", config.CompiledIndexCacheDirectory);

            var ravenDbServer = new RavenDbServer(config);

            ravenDbServer.Initialize();

            ravenDbServer.DocumentStore.DefaultDatabase = Dependency.OnAppSettingsValue("Logse/RavenName").Value;
            ravenDbServer.DocumentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(ravenDbServer.DocumentStore.DefaultDatabase);

            ravenDbServer.FilesStore.DefaultFileSystem = Dependency.OnAppSettingsValue("Logse/RavenName").Value;
            ravenDbServer.FilesStore.AsyncFilesCommands.Admin.EnsureFileSystemExistsAsync(ravenDbServer.FilesStore.DefaultFileSystem).Wait();

            IndexCreation.CreateIndexes(this.GetType().Assembly, ravenDbServer.DocumentStore);

            if (bool.Parse(Dependency.OnAppSettingsValue("Logse/EnableRavenHttpServer").Value))
            {
                ravenDbServer.EnableHttpServer();
            }

            return ravenDbServer;
        }
        #endregion
    }
}