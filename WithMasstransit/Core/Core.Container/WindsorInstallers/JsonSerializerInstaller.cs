using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rabbit.Core;

namespace Core.Container.WindsorInstallers
{
    public class JsonSerializerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyInDirectory(new AssemblyFilter(ConfigurationManager.AppSettings["ExtensionFolder"] ?? string.Empty))
                .BasedOn<IJsonSerializer>()
                .WithService.AllInterfaces()
                .LifestyleSingleton());
        }
    }
}
