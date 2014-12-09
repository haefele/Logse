using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Hosting;
using Raven.Abstractions.Extensions;
using Xemio.Logse.Server;

namespace Xemio.Logse.Hosts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(Dependency.OnAppSettingsValue("Logse/Addresses").Value.Split('|'));

            using (WebApp.Start<Startup>(startOptions))
            {
                System.Console.WriteLine("Xemio Logse Web-API started.");
                System.Console.WriteLine("Press any key to close.");

                System.Console.ReadLine();
            }
        }
    }
}
