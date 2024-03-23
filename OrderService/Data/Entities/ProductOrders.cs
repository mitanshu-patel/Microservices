using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Data.Entities
{
    public class ProductOrders
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public RemoteProduct Product { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
