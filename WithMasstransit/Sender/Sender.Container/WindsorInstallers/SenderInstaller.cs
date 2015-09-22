using System;
using System.Configuration;
using System.Diagnostics;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Log4NetIntegration;

namespace Sender.Container.WindsorInstallers
{
    public class SenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var receiveForm = ConfigurationManager.AppSettings["ServiceBus.ReceiveFrom"];
            var serviceBus = ServiceBusFactory.New(config =>
            {

                if (receiveForm.Contains("rabbitmq"))
                {

                    config.UseRabbitMq(
                        s => s.ConfigureHost(
                            new Uri(receiveForm),
                           
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

                config.ReceiveFrom(ConfigurationManager.AppSettings["ServiceBus.ReceiveFrom"]);
                config.UseControlBus();
            
                config.UseJsonSerializer();
                config.SetNetwork("Test");
                config.Subscribe(subs => subs.LoadFrom(container));
                config.EnableMessageTracing();
                config.UseLog4Net("MassTransitLog.config");
                config.EnableRemoteIntrospection();
            });

            var probe = serviceBus.Probe();
            serviceBus.WriteIntrospectionToConsole();

            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            container.Register(
                Component.For<IServiceBus>().Instance(serviceBus).LifestyleSingleton());

        }
    }
}