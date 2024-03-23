using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Data;
using ProductService.Messages;
using ProductService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.EditProduct
{
    public class EditProductHandler : IHandler<EditProductCommand, CustomResponse<EditProductResult>>
    {
        private readonly ProductContext dbContext;
        private readonly IMessageDeliveryService messageDeliveryService;
        public EditProductHandler(ProductContext dbContext, IMessageDeliveryService messageDeliveryService)
        {
            this.dbContext = dbContext;
            this.messageDeliveryService = messageDeliveryService;
        }
        public async Task<CustomResponse<EditProductResult>> Handle(EditProductCommand command)
        {
            var product = await this.dbContext.Products.FirstOrDefaultAsync(t => t.Id == command.ProductId);
            if (product == null)
            {
                return CustomHttpResult.NotFound<EditProductResult>("Product doesn't exist");
            }

            product.Name = command.ProductDetail.Name;
            product.Description = command.ProductDetail.Description;
            product.Price = command.ProductDetail.Price;

            dbContext.Entry(product).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            await this.messageDeliveryService.PublishMessageAsync(new ProductDetailUpdated(product.Id, product.Name, product.Description, product.Price), "productserviceevents");
            return CustomHttpResult.Ok(new EditProductResult());
        }
    }
}
