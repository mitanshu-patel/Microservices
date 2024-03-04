using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Data.Entities;

namespace UserService.Data.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasIndex(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(50);
            builder.Property(c => c.MobileNo).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(20);
            builder.Property(c => c.Email).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(50);
            builder.Property(c => c.Password).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(int.MaxValue);
        }
    }
}
