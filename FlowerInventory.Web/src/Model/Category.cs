namespace FlowerInventorySystem.FlowerInventory.Web.Model;

public class Category : BaseModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Flower> Flowers { get; init; } = new List<Flower>();
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public string? ImagePath { get; set; }
}