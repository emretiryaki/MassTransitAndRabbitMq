using System;

using MassTransit;
using Message;

namespace Receiver.Consumer
{
    public class ProductConsumer : Consumes<Product>.All
    {
       
        public void Consume(Product message)
        {
            Console.WriteLine("CorrelationId:" + message.CorrelationId + " Product send....");
            Console.Write("Product : {0} , {1} ", message.Id, message.ProductName);
        }
    }
}
