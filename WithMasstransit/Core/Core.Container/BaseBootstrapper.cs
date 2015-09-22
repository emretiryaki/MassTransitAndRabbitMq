using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Castle.Windsor.Installer;
using Rabbit.Core;

namespace Core.Container
{
    public abstract class BaseBootstrapper
    {
        public IWindsorContainer container = new WindsorContainer();
        public IKernel kernel { get; private set; }

        public BaseBootstrapper(bool isHttpEnabled = false)
        {

           
            this.container.AddFacility<TypedFactoryFacility>().AddFacility<WcfFacility>(f => f.CloseTimeout = TimeSpan.Zero);

            ServiceMetadataBehavior metadata = new ServiceMetadataBehavior();

            if (isHttpEnabled)
            {
                metadata.HttpGetEnabled = true;
                metadata.HttpsGetEnabled = true;
            }

            ServiceDebugBehavior returnFaults = new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true };

            this.container.Register(
                Component.For<IServiceBehavior>().Instance(metadata),
                Component.For<IServiceBehavior>().Instance(returnFaults));

            this.container.Install(FromAssembly.InDirectory(new AssemblyFilter(ConfigurationManager.AppSettings["ExtensionFolder"] ?? string.Empty)));

            this.kernel = this.container.Kernel;

            container.Register(Component.For<IJsonSerializer>().ImplementedBy<JsonSerializer>().Named("GeneralJsonSerializer").LifeStyle.PerWcfOperation());
     


            CheckCastleRegisterComponent();


        }

        protected void CheckCastleRegisterComponent()
        {
            var log = container.Resolve<ILogger>();

            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var diagnostics = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>();

            var handlers = diagnostics.Inspect();

            if (handlers.Any())
            {
                var message = new StringBuilder();
                var inspector = new DependencyInspector(message);

                foreach (IExposeDependencyInfo handler in handlers)
                {
                    handler.ObtainDependencyDetails(inspector);
                }

                log.Log(message.ToString(), LogType.Error);
                Console.WriteLine(message.ToString());
            }

            container.Release(log);

        }


    }
}
