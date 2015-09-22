using System.Configuration;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core;

namespace STC.Core.Container.WindsorInstallers
{
    public class LoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILogger>()
                    .ImplementedBy<Log4NetLogger>()
                    .LifeStyle.PerThread);

            string logFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ExtensionFolder"] ?? string.Empty, "log4net.config");
            Log4NetLogger.Init(logFilePath);

            ILogger logger = container.Resolve<ILogger>();
        }
    }
}
