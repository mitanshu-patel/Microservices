using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Data.EntityConfigurations
{
    public class ProductOrderEntityConfiguration : IEntityTypeConfiguration<ProductOrders>
    {
        public void Configure(EntityTypeBuilder<ProductOrders> builder)
        {
            if(builder == null)
            {
                throw new NotImplementedException();
            }

            builder.HasIndex(e => e.Id);
            builder.HasOne(c => c.Order).WithMany(v => v.ProductOrders).HasForeignKey(v => v.OrderId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
