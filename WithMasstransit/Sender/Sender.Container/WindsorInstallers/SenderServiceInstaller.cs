
using System;
using System.Configuration;
using System.ServiceModel;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sender.Container.Interceptors;
using Sender.Contract;

namespace Sender.Container.WindsorInstallers
{
    public class SenderServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            string serviceHostUrl = ConfigurationManager.AppSettings["SenderServiceHostUrl"];

            container.Register(
                Component.For<ISenderService>()
                    .ImplementedBy<SenderService>().Interceptors<MessageLoggerInterceptor>()
                    .LifeStyle.PerWcfOperation()
                    .AsWcfService(new DefaultServiceModel().AddEndpoints(WcfEndpoint.BoundTo(
                   new BasicHttpBinding()
                   {
                       Security = new BasicHttpSecurity()
                       {
                           Mode = BasicHttpSecurityMode.None
                       },
                       MaxReceivedMessageSize = Int32.MaxValue,
                       MaxBufferPoolSize = Int32.MaxValue,
                       ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                       {
                           MaxStringContentLength = Int32.MaxValue
                       }
                   }).At(serviceHostUrl)).AddBaseAddresses(serviceHostUrl).PublishMetadata(o => o.EnableHttpGet())));
        }
    }
}