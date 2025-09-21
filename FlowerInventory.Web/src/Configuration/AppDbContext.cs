using FlowerInventorySystem.FlowerInventory.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace FlowerInventory.Web.Configuration;
/// <summary>
///     Creates the database context. 
///     Sets the rule for objects.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Flower> Flowers => Set<Flower>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PublicId).IsRequired().HasDefaultValueSql("NEWID()");
            e.HasIndex(x => x.PublicId).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).HasMaxLength(400);
            e.Property(x => x.ImagePath).HasMaxLength(500);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.DisplayOrder).HasDefaultValue(0);
            e.HasIndex(x => new { x.IsActive, x.DisplayOrder });
        });

        b.Entity<Flower>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PublicId).IsRequired().HasDefaultValueSql("NEWID()");
            e.HasIndex(x => x.PublicId).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(120);
            e.Property(x => x.Type).IsRequired().HasMaxLength(80);
            e.Property(x => x.Price).HasColumnType("decimal(10,2)");
            e.Property(x => x.ImagePath).HasMaxLength(500);
            e.Property(x => x.Sku).IsRequired().HasMaxLength(40);
            e.HasIndex(x => x.Sku).IsUnique();
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.StockQuantity).HasDefaultValue(0);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.HasOne(f => f.Category)
                .WithMany(c => c.Flowers)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            e.Navigation(f => f.Category).AutoInclude();
        });
    }
}