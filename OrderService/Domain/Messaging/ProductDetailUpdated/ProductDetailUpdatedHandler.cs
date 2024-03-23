using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Messaging.ProductDetailUpdated
{
    public class ProductDetailUpdatedHandler : IHandler<ProductDetailUpdatedCommand, CustomResponse<ProductDetailUpdatedResult>>
    {
        private readonly OrderContext dbContext;

        public ProductDetailUpdatedHandler(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<ProductDetailUpdatedResult>> Handle(ProductDetailUpdatedCommand command)
        {
            var productDetail = await this.dbContext.RemoteProducts.FindAsync(command.ProductDetailUpdated.ProductId);
            if(productDetail == null)
            {
                productDetail = new Data.Entities.RemoteProduct
                {
                    Price = command.ProductDetailUpdated.Price,
                    Id = command.ProductDetailUpdated.ProductId,
                    Name = command.ProductDetailUpdated.Name,
                    Description = command.ProductDetailUpdated.Description
                };

                this.dbContext.RemoteProducts.Add(productDetail);
            }
            else
            {
                productDetail.Price = command.ProductDetailUpdated.Price;
                productDetail.Id = command.ProductDetailUpdated.ProductId;
                productDetail.Name = command.ProductDetailUpdated.Name;
                productDetail.Description = command.ProductDetailUpdated.Description;
                this.dbContext.Entry(productDetail).State = EntityState.Modified;
            }

            await this.dbContext.SaveChangesAsync();

            return CustomHttpResult.Ok(new ProductDetailUpdatedResult());
        }
    }
}
