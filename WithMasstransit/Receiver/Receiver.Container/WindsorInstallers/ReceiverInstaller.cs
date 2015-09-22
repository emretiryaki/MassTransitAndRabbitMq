using System;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Log4NetIntegration;

namespace Receiver.Container.WindsorInstallers
{
    public class ReceiverInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            var subscriptionServiceUrl = ConfigurationManager.AppSettings["ServiceBus.SubscriptionsService"];

            var receiveFromUrl = ConfigurationManager.AppSettings["ServiceBus.ReceiveFrom"];


            var serviceBus = ServiceBusFactory.New(config =>
            {
                config.SetCreateMissingQueues(true);
                config.SetDefaultRetryLimit(5);

                if (receiveFromUrl.Contains("rabbitmq"))
                {
                    config.UseRabbitMq(
                        s => s.ConfigureHost(
                            new Uri(receiveFromUrl),
                            con =>
                            {
                                con.SetUsername(ConfigurationManager.AppSettings["ServiceBus.Username"]);
                                con.SetPassword(ConfigurationManager.AppSettings["ServiceBus.Password"]);
                            }));
                }
                else
                {
                    throw new ArgumentException("Transport provider doesnt support");
                }


                config.SetNetwork("StcLoyality");
                config.ReceiveFrom(receiveFromUrl);
                config.UseControlBus();
                config.UseJsonSerializer();
             

           
                config.Subscribe(subs => subs.LoadFrom(container));
                config.EnableMessageTracing();
                config.EnableRemoteIntrospection();
                config.SetConcurrentConsumerLimit(1);

                config.UseLog4Net("MassTransitLog.config");
            });

            var probe = serviceBus.Probe();

           
            serviceBus.WriteIntrospectionToConsole();

            container.Register(Component.For<IServiceBus>().Instance(serviceBus).LifestyleSingleton());
        }

        #endregion
    }
}