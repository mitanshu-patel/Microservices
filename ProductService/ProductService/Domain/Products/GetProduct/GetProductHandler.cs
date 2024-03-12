using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Data;
using ProductService.Domain.Products.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.GetProduct
{
    public class GetProductHandler : IHandler<GetProductQuery, CustomResponse<GetProductResult>>
    {
        private readonly ProductContext dbContext;

        public GetProductHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CustomResponse<GetProductResult>> Handle(GetProductQuery command)
        {
            var product = await this.dbContext.Products.Where(t => t.Id == command.ProductId)
                                                        .Select(v => new ProductDetailDto(v.Id, v.Name, v.Description, v.Price))
                                                        .FirstOrDefaultAsync();

            if(product == null)
            {
                return CustomHttpResult.NotFound<GetProductResult>("Product doesn't exist");
            }

            return CustomHttpResult.Ok(new GetProductResult(product));
        }
    }
}
