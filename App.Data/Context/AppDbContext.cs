 using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<CartEntity> Carts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();


            modelBuilder.Entity<ProductEntity>().HasData(
                new ProductEntity { Id = 1, Name = "Asus Laptop", Price = 25000.50m },
                new ProductEntity { Id = 2, Name = "Hp Laptop", Price = 20000.00m },
                new ProductEntity { Id = 3, Name = "Monster Laptop", Price = 30000.15m },
                new ProductEntity { Id = 4, Name = "MSI Laptop", Price = 40000.50m },
                new ProductEntity { Id = 5, Name = "Xiaomi Telefon", Price = 10000.00m },
                new ProductEntity { Id = 6, Name = "Samsung Telefon", Price = 16000.00m },
                new ProductEntity { Id = 7, Name = "Huawei Telefon", Price = 17000.99m },
                new ProductEntity { Id = 8, Name = "Iphone Telefon", Price = 60000.15m },
                new ProductEntity { Id = 9, Name = "Mouse", Price = 3000.25m },
                new ProductEntity { Id = 10, Name = "Klavye", Price = 4500.99m }
            );

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity { Id = 1, Name = "Efe Can", Surname = "EKEN", Email = "eken@eken.com", Password = "12345" },
                new UserEntity { Id = 2, Name = "Gökhan", Surname = "Türkmen", Email = "turkmen@turkmen.com", Password = "12345" }
            );

            modelBuilder.Entity<CartEntity>().HasData(
                new CartEntity { Id = 1, UserId = 1, CreatedAt = DateTime.Now, Name = "DefaultCart" },
                new CartEntity { Id = 2, UserId = 2, CreatedAt = DateTime.Now, Name = "DefaultCart" }
            );
        }
    }
}