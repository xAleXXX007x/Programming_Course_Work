using ElectronicsShopDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShopDatabase
{
    public class ElectronicsShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Initial Catalog=ElectronicsShopDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Client> Clients { set; get; }

        public virtual DbSet<Order> Orders { set; get; }

        public virtual DbSet<OrderProduct> OrderProducts { set; get; }

        public virtual DbSet<Payment> Payments { set; get; }

        public virtual DbSet<Product> Products { set; get; }
    }
}
