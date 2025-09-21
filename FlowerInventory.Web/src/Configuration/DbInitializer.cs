using FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace FlowerInventorySystem.FlowerInventory.Web.Configuration;

public static class DbInitializer
{
    
    public static async Task InitializeAsync(AppDbContext db, CancellationToken ct = default)
    {
        var hasMigrations = db.Database.GetMigrations().Any();

        if (hasMigrations)
            await db.Database.MigrateAsync(ct);
        else
            await db.Database.EnsureCreatedAsync(ct);

        await SeedAsync(db, ct);
    }

    private static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        var roses = await EnsureCategoryAsync(db, Guid.NewGuid(), "Roses", "Classic rose varieties", true, 1, null, ct);
        var tulips = await EnsureCategoryAsync(db, Guid.NewGuid(), "Tulips", "Seasonal tulip stems", true, 2, null, ct);
        var lilies = await EnsureCategoryAsync(db, Guid.NewGuid(), "Lilies", "Asiatic & Oriental", true, 3, null, ct);

        await EnsureFlowerAsync(db, Guid.NewGuid(),
            "Red Rose", "Single", 2.50m, roses.Id,
            "ROSE-RED-001", 200, true,
            "Classic single red rose.", null, ct);

        await EnsureFlowerAsync(db, Guid.NewGuid(),
            "Rose Bouquet", "Bouquet", 24.99m, roses.Id,
            "ROSE-BOU-010", 25, true,
            "Bouquet of 10 mixed roses.", null, ct);

        await EnsureFlowerAsync(db, Guid.NewGuid(),
            "Yellow Tulip", "Single", 1.80m, tulips.Id,
            "TULIP-YEL-001", 300, true,
            "Bright yellow tulip stem.", null, ct);

        await EnsureFlowerAsync(db, Guid.NewGuid(),
            "Tulip Bunch", "Bouquet", 12.00m, tulips.Id,
            "TULIP-BUN-012", 40, true,
            "Bunch of 12 tulips.", null, ct);

        await EnsureFlowerAsync(db, Guid.NewGuid(),
            "Asiatic Lily", "Stem", 3.20m, lilies.Id,
            "LILY-ASI-001", 120, true,
            "Elegant Asiatic lily stem.", null, ct);

        await db.SaveChangesAsync(ct);
    }

    private static async Task<Category> EnsureCategoryAsync(
        AppDbContext db, Guid publicId, string name, string? description,
        bool isActive, int displayOrder, string? imagePath, CancellationToken ct)
    {
        var existing = await db.Categories.FirstOrDefaultAsync(c => c.PublicId == publicId, ct);
        if (existing is not null) return existing;

        var cat = new Category
        {
            PublicId = publicId,
            Name = name,
            Description = description,
            IsActive = isActive,
            DisplayOrder = displayOrder,
            ImagePath = imagePath
        };

        db.Categories.Add(cat);
        await db.SaveChangesAsync(ct);
        return cat;
    }

    private static async Task EnsureFlowerAsync(
        AppDbContext db, Guid publicId, string name, string type, decimal price, int categoryId,
        string? sku, int stockQty, bool isActive, string? description, string? imagePath,
        CancellationToken ct)
    {
        var existing = await db.Flowers.FirstOrDefaultAsync(f => f.PublicId == publicId, ct);
        if (existing is not null) return;

        var flower = new Flower
        {
            PublicId = publicId,
            Name = name,
            Type = type,
            Price = price,
            CategoryId = categoryId,
            Sku = sku,
            StockQuantity = stockQty,
            IsActive = isActive,
            Description = description,
            ImagePath = imagePath
        };

        db.Flowers.Add(flower);
    }
}