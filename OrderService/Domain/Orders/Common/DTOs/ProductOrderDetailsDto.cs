using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Common.DTOs
{
    public record ProductOrderDetailsDto
    {
        public int ProductId { get; init; }

        public decimal Price { get; init; }

        public string Name { get; init; }

        public int Quantity { get; init; }
    }
}
