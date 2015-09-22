using System.Configuration;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Core.Container.WindsorInstallers
{
    public class InterceptorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyInDirectory(new AssemblyFilter(ConfigurationManager.AppSettings["ExtensionFolder"] ?? string.Empty))
                .BasedOn<IInterceptor>()
                .WithService.AllInterfaces()
                .LifestylePerThread());
        }
    }
}
