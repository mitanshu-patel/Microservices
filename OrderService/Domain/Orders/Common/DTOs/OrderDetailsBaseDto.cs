using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Common.DTOs
{
    public record OrderDetailsBaseDto
    {
        public int OrderId { get; init; }
        public decimal TotalPrice { get; init; }

        public DateTime OrderDate { get; init; }

        public List<ProductOrderDetailsDto> ProductOrders { get; init; }
    }
}
