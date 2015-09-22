using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Container.ServiceInstaller;

namespace Receiver.App
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceStarter.StartApplication<Service>(args);
        }
    }
}
