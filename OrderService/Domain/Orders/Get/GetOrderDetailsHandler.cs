using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Data;
using OrderService.Domain.Orders.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Get
{
    public class GetOrderDetailsHandler : IHandler<GetOrderDetailsQuery, CustomResponse<GetOrderDetailsResult>>
    {
        private readonly OrderContext dbContext;
        public GetOrderDetailsHandler(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<GetOrderDetailsResult>> Handle(GetOrderDetailsQuery command)
        {
            var orderDetail = await this.dbContext.Orders.Where(t => t.Id == command.OrderId).Select(v=> new OrderDetailsDto
            {
                CustomerName = v.User.Name,
                TotalPrice = v.TotalPrice,
                OrderDate = v.OrderDate,
                ProductOrders = v.ProductOrders.Select(y=> new ProductOrderDetailsDto
                {
                    Price = y.Product.Price,
                    ProductId = y.ProductId,
                    Name = y.Product.Name,
                    Quantity = y.Quantity,
                }).ToList(),
            }).FirstOrDefaultAsync();

            if(orderDetail == null)
            {
                return CustomHttpResult.NotFound<GetOrderDetailsResult>("Order doesn't exist");
            }

            return CustomHttpResult.Ok(new GetOrderDetailsResult(orderDetail));
        }
    }
}
