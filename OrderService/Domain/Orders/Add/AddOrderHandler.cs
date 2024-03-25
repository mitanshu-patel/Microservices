using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Data;
using OrderService.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Orders.Add
{
    public class AddOrderHandler : IHandler<AddOrderCommand, CustomResponse<AddOrderResult>>
    {
        private readonly OrderContext dbContext;
        public AddOrderHandler(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<AddOrderResult>> Handle(AddOrderCommand command)
        {
            var productIds = command.ProductOrders.Select(t => t.ProductId);
            var productPrices = await this.dbContext.RemoteProducts.Where(t => productIds.Contains(t.Id)).Select(v => new
            {
                Id = v.Id,
                Price = v.Price
            }).ToListAsync();

            var existingProductIds = productPrices.Select(t => t.Id).ToList();
            var invalidProductExist = productIds.Any(t => !existingProductIds.Contains(t));

            if (invalidProductExist)
            {
                return CustomHttpResult.BadRequest<AddOrderResult>("Order contains one or more invalid products, please try again");
            }

            var totalPrice = 0m;
            foreach(var productPrice in productPrices)
            {
                var quantity = command.ProductOrders.Where(t => t.ProductId == productPrice.Id).Select(v => v.Quantity).FirstOrDefault();
                totalPrice += (productPrice.Price * quantity);
            }

            var order = new Order
            {
                UserId = command.UserId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
            };

            this.dbContext.Orders.Add(order);
            await this.dbContext.SaveChangesAsync();

            this.dbContext.ProductOrders.AddRange(command.ProductOrders.Select(t => new ProductOrders()
            {
                ProductId = t.ProductId,
                OrderId = order.Id,
                Quantity = t.Quantity
            }));

            await this.dbContext.SaveChangesAsync();

            return CustomHttpResult.Ok(new AddOrderResult(order.Id));
        }
    }
}
