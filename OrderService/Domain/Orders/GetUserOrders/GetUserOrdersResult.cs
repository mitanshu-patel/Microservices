using OrderService.Domain.Orders.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.GetUserOrders
{
    public record GetUserOrdersResult(List<OrderDetailsBaseDto> UserOrders);
}
