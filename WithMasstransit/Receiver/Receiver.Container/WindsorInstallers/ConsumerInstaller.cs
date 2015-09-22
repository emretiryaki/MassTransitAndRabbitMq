using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using Receiver.Consumer;

namespace Receiver.Container.WindsorInstallers
{
    public class ConsumerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {


            container.Register(Classes.FromThisAssembly().BasedOn<IConsumer>().LifestyleTransient(),
                               Component.For<ProductConsumer>().LifestyleTransient());


        }

        #endregion
    }
}