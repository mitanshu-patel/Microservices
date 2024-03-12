using ProductService.Domain.Products.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.EditProduct
{
    public record EditProductCommand(int ProductId, ProductDetailDto ProductDetail);
}
