using SIS.Framework;
using SIS.Framework.Routers;
using SIS.WebServer;
using System;

namespace Sis.Demo2
{
    class Launcher
    {
        static void Main(string[] args)
        {
            var server = new Server(80, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}
