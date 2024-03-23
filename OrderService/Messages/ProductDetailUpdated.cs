using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Messages
{
    public record ProductDetailUpdated(int ProductId, string Name, string Description, decimal Price);
}
