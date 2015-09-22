using System;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace Core.Container.ServiceInstaller
{
    public static class ServiceStarter
    {
        public static void CheckServiceRegisteration(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                {
                    SelfInstaller.InstallMe();
                    Environment.Exit(0);
                }
                else if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                {
                    SelfInstaller.UninstallMe();
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Servis implementasyonunu windows service olarak args parametresine bakarak kurulum yapar.
        /// Kullanıcı interaktivitesine bakarak, console uygulaması yada windows service olarak T ile gönderilen
        /// ServiceBase den türemiş classları çalıştırır
        /// </summary>
        /// <typeparam name="T">ServiceBase türemiş,yayınlanacak servis implementasyonlarını</typeparam>
        /// <param name="args">Program.Main(string[] args) gönderilir</param>
        public static void StartApplication<T>(string[] args) where T : ServiceBase, new()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var section = config.GetSection("ProtectedSection");

            if (section != null && section.SectionInformation.IsProtected == false)
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");

                config.Save();
            }

            var servicesToRun = new ServiceBase[]
                                     {
                                         new T()
                                     };

            StartApplication(servicesToRun,args);
        }

        /// <summary>
        /// Servis implementasyonunu windows service olarak args parametresine bakarak kurulum yapar.
        /// Kullanıcı interaktivitesine bakarak, console uygulaması yada windows service olarak T ile gönderilen
        /// ServiceBase den türemiş classları çalıştırır
        /// </summary>
        /// <typeparam name="ServiceBase[]">ServiceBase türemiş,yayınlanacak servis implementasyonlarının tümünü çalıştırmak için gönderilen dizi
        /// Örnek: new ServiceBase[]{ new Service1(), new Service2(), new Service3()} 
        /// </typeparam>
        /// <param name="args">Program.Main(string[] args) gönderilir</param>
        public static void StartApplication(ServiceBase[] services, string[] args)
        {
            //Check service registeration before go to
            CheckServiceRegisteration(args);

            if (Environment.UserInteractive)
                RunAsConsole(services);
            else
                ServiceBase.Run(services);
        }

        private static void RunAsConsole(ServiceBase[] servicesToRun)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Services running in console mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("Started");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Press escape key to stop the services and end the process...");
            Console.ReadKey();
            while (true)
            {
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape)
                    break;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.Write("Stopped {0}...", service.ServiceName);
            }

            Console.WriteLine("All services stopped.");

            Thread.Sleep(1000);
        }
    }
}
