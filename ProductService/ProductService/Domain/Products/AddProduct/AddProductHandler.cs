using ProductService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Products.AddProduct
{
    public class AddProductHandler
    {
        private readonly ProductContext dbContext;
        public AddProductHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
