using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_SharpAi.Models;
using System.Collections.Generic;


namespace Zadanie_Rekrutacyjne_SharpAi.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.Name).HasColumnName("name");
            entity.Property(p => p.Price).HasColumnName("price");
        });


        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Orders)
            .WithMany(o => o.Products)
            .UsingEntity<Dictionary<string, object>>(
                "order_products", 
                j => j.HasOne<Order>().WithMany().HasForeignKey("order_id"),
                j => j.HasOne<Product>().WithMany().HasForeignKey("product_id"),
                j =>
                {
                    j.ToTable("orders_products");
                    j.HasKey("order_id", "product_id");
                }
            );
    }
}