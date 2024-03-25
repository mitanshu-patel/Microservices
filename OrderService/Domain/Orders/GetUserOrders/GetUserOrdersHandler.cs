using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Data;
using OrderService.Domain.Orders.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.GetUserOrders
{
    public class GetUserOrdersHandler : IHandler<GetUserOrdersQuery, CustomResponse<GetUserOrdersResult>>
    {
        private readonly OrderContext dbContext;

        public GetUserOrdersHandler(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<GetUserOrdersResult>> Handle(GetUserOrdersQuery command)
        {
            var userExist = await this.dbContext.RemoteUsers.AnyAsync(t=>t.Id == command.UserId);
            if (!userExist)
            {
                return CustomHttpResult.NotFound<GetUserOrdersResult>("User doesn't exist");
            }
            var userOrderList = await this.dbContext.Orders.Where(t => t.UserId == command.UserId).Select(y=> new OrderDetailsBaseDto
            {
                OrderId = y.Id,
                TotalPrice = y.TotalPrice,
                OrderDate = y.OrderDate,
                ProductOrders = y.ProductOrders.Select(v=> new ProductOrderDetailsDto
                {
                    Name = v.Product.Name,
                    Price = v.Product.Price,
                    ProductId = v.ProductId,
                    Quantity = v.Quantity,
                }).ToList(),
            }).ToListAsync();

            return CustomHttpResult.Ok(new GetUserOrdersResult(userOrderList));
        }
    }
}
