namespace FlowerInventory.Web.Model;

public class Flower : BaseModel
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; init; }
    public string? ImagePath { get; set; }
    public string? Sku { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
}