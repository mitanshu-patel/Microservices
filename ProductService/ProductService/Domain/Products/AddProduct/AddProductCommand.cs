using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.AddProduct
{
    public record AddProductCommand(string Name, decimal Price, string Description);
}
