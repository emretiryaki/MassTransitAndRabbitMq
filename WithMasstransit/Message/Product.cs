using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Message
{
    [Serializable]
    public class Product : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public int Id { get; set; }

        public string ProductName { get; set; }
    }
}
