namespace FlowerInventorySystem.FlowerInventory.Web.Model;

public abstract class BaseModel
{
    public int Id { get; set; }
    public Guid PublicId { get; init; } = Guid.NewGuid();
}