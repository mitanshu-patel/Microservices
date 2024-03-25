using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Common.DTOs
{
    public record OrderDetailsDto : OrderDetailsBaseDto
    {
        public string CustomerName { get; init; }
    }
}
