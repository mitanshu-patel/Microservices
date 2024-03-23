using Microsoft.EntityFrameworkCore;
using OrderService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<RemoteUser> RemoteUsers { get; set; }
        public DbSet<RemoteProduct> RemoteProducts { get; set; }
        public DbSet<ProductOrders> ProductOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder), "Model Builder Should not be null.");
            }

            modelBuilder.Entity<RemoteUser>()
           .Property(p => p.Id)
           .ValueGeneratedNever();

            modelBuilder.Entity<RemoteProduct>()
           .Property(p => p.Id)
           .ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
