using System;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Hosting;
using Raven.Abstractions.Extensions;
using Xemio.Logse.Server;

namespace Xemio.Logse.Hosts.Service
{
    public class WindowsService
    {
        #region Fields
        private IDisposable _webApp;
        #endregion

        #region Methods
        /// <summary>
        /// Executed when the service starts.
        /// </summary>
        public void OnStart()
        {
            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(Dependency.OnAppSettingsValue("Logse/Addresses").Value.Split('|'));

            this._webApp = WebApp.Start<Startup>(startOptions);
        }
        /// <summary>
        /// Executed when the service stops.
        /// </summary>
        public void OnStop()
        {
            if (this._webApp != null)
                this._webApp.Dispose();
        }
        #endregion
    }
}