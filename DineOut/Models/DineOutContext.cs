using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DineOut.Models
{
    public class DineOutContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> Order_Item { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<RestaurantProfile> RestaurantProfile { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=salt.db.elephantsql.com; Port=5432; Database=jliofnqg;Username=jliofnqg; Password=peZJAtdSrAdJIte3YjKqacTbL2PptxD4");

    }
}
