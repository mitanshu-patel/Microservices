using OrderService.Domain.Orders.Add.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Add
{
    public record AddOrderCommand(List<ProductOrderDto> ProductOrders, int UserId);
}
