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
    public class RemoteUserEntityConfiguration : IEntityTypeConfiguration<RemoteUser>
    {
        public void Configure(EntityTypeBuilder<RemoteUser> builder)
        {
            if(builder == null)
            {
                throw new NotImplementedException();
            }

           builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedNever();
        }
    }
}
