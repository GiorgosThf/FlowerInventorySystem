namespace FlowerInventory.Web.Dto;

public class FlowerDto(Guid publicId, string name, string type, decimal price, Guid categoryId, string? imagePath)
{
    public static FlowerDtoBuilder Builder()
    {
        return new FlowerDtoBuilder();
    }

    public class FlowerDtoBuilder
    {
        private Guid _categoryId;
        private string? _imagePath;
        private string _name = string.Empty;
        private decimal _price;
        private Guid _publicId = Guid.Empty;
        private string _type = string.Empty;

        public FlowerDtoBuilder WithPublicId(Guid id)
        {
            _publicId = id;
            return this;
        }

        public FlowerDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public FlowerDtoBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public FlowerDtoBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public FlowerDtoBuilder WithCategory(Guid categoryId)
        {
            _categoryId = categoryId;
            return this;
        }

        public FlowerDtoBuilder WithImage(string? path)
        {
            _imagePath = path;
            return this;
        }

        public FlowerDto Build()
        {
            return new FlowerDto(_publicId, _name, _type, _price, _categoryId, _imagePath);
        }
    }
}