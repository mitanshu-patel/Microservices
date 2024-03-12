using ProductService.Domain.Products.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.GetProducts
{
    public record GetProductsResult(List<ProductDetailDto> Products);
}
