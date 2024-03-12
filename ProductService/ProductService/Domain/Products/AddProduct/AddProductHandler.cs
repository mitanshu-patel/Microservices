using ProductService.Common;
using ProductService.Data;
using ProductService.Data.Entities;
using ProductService.Messages;
using ProductService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.AddProduct
{
    public class AddProductHandler : IHandler<AddProductCommand, CustomResponse<AddProductResult>>
    {
        private readonly ProductContext dbContext;
        private readonly IMessageDeliveryService messageDeliveryService;
        public AddProductHandler(ProductContext dbContext, IMessageDeliveryService messageDeliveryService)
        {
            this.dbContext = dbContext;
            this.messageDeliveryService = messageDeliveryService;
        }

        public async Task<CustomResponse<AddProductResult>> Handle(AddProductCommand command)
        {
            var product = new Product()
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
            };

            this.dbContext.Products.Add(product);
            await this.dbContext.SaveChangesAsync();

            await this.messageDeliveryService.PublishMessageAsync(new ProductDetailUpdated(product.Id, product.Name, product.Description, product.Price), "productevents");

            return CustomHttpResult.Ok(new AddProductResult(product.Id));
        }
    }
}
