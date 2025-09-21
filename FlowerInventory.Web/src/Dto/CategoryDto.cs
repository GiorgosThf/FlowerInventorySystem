namespace FlowerInventorySystem.FlowerInventory.Web.Dto;

public class CategoryDto(Guid? publicId, string? name, string? description)
{
    public Guid? PublicId => publicId;
    public string? Name => name;
    public string? Description => description;

    public static CategoryBuilder Builder()
    {
        return new CategoryBuilder();
    }

    public class CategoryBuilder
    {
        private string _description = string.Empty;
        private string _name = string.Empty;
        private Guid _publicId = Guid.Empty;

        public CategoryBuilder WithPublicId(Guid id)
        {
            _publicId = id;
            return this;
        }

        public CategoryBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CategoryBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public CategoryDto Build()
        {
            return new CategoryDto(_publicId, _name, _description);
        }
    }
}