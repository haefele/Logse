using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Owin;
using Raven.Database.Server.WebApi.Handlers;
using Xemio.Logse.Server.WebApi.ContentNegotiation;
using Xemio.Logse.Server.Windsor;

namespace Xemio.Logse.Server
{
    public class Startup
    {
        #region Methods
        /// <summary>
        /// Configurations this application.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            this.UseCors(appBuilder);
            this.UseWebApi(appBuilder);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Makes the app enable CORS.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        private void UseCors(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);
        }
        /// <summary>
        /// Makes the app use Web-API.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        private void UseWebApi(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            this.ConfigureWindsor(config);
            this.ConfigureMessageHandlers(config);
            this.ConfigureRoutes(config);
            this.ConfigureAllowOnlyJson(config);

            appBuilder.UseWebApi(config);
        }
        /// <summary>
        /// Configures the castle windsor IoC container.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureWindsor(HttpConfiguration config)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());

            config.DependencyResolver = new WindsorResolver(container);
        }
        /// <summary>
        /// Configures the message handlers.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureMessageHandlers(HttpConfiguration config)
        {
            if (bool.Parse(Dependency.OnAppSettingsValue("Logse/CompressResponses").Value))
            {
                config.MessageHandlers.Add(new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
            }
        }
        /// <summary>
        /// Configures the routes.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
        /// <summary>
        /// Configures the config to allow only json requests.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureAllowOnlyJson(HttpConfiguration config)
        {
            if (bool.Parse(Dependency.OnAppSettingsValue("Logse/FormatResponses").Value))
            {
                config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            }

            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(config.Formatters.JsonFormatter));
        }
        #endregion
    }
}
