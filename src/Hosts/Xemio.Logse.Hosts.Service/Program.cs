using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Xemio.Logse.Hosts.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(f =>
            {
                f.SetServiceName("LogseWebAPI");
                f.SetDisplayName("Logse Web-API");
                f.SetDescription("This windows service hosts the Logse Web-API.");
                f.StartAutomatically();

                f.Service<WindowsService>(s =>
                {
                    s.ConstructUsing(() => new WindowsService());

                    s.WhenStarted(d => d.OnStart());
                    s.WhenStopped(d => d.OnStop());
                });
            });
        }
    }
}
