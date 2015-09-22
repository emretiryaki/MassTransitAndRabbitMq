
using System;
using MassTransit;
using Message;
using Sender.Contract;

namespace Sender
{
    public class SenderService : ISenderService
    {

        private readonly IServiceBus _serviceBus;

        public SenderService(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public ProductResponse AddProduct(ProductRequest productRequest)
        {           _serviceBus.Publish(new Product
            {
                CorrelationId = Guid.NewGuid(),
                ProductName = productRequest.ProductName,
                Id = productRequest.ProductId
            });
            return new ProductResponse();
        }
    }
}
