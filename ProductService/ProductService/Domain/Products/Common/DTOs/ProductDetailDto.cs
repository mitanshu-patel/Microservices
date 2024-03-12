using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.Common.DTOs
{
    public record ProductDetailDto(int ProductId, string Name, string Description, decimal Price);
}
