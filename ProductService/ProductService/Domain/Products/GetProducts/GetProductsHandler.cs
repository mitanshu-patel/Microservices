using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Data;
using ProductService.Domain.Products.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.GetProducts
{
    public class GetProductsHandler : IHandler<GetProductsQuery, CustomResponse<GetProductsResult>>
    {
        private readonly ProductContext dbContext;
        public GetProductsHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<GetProductsResult>> Handle(GetProductsQuery command)
        {
            var products = await this.dbContext.Products.Select(t => new ProductDetailDto(t.Id, t.Name, t.Description, t.Price)).ToListAsync();

            return CustomHttpResult.Ok(new GetProductsResult(products));
        }
    }
}
