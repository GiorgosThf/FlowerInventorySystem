using FlowerInventorySystem.FlowerInventory.Web.Dto;
using FlowerInventorySystem.FlowerInventory.Web.Model;

namespace FlowerInventorySystem.FlowerInventory.Web.Utils;

public class ModelMapper
{
    public static FlowerDto MapToDto(Flower flower)
    {
        return FlowerDto.Builder().Build();
    }


    public static CategoryDto MapToDto(Category category)
    {
        return CategoryDto.Builder()
            .WithPublicId(category.PublicId)
            .WithDescription(category.Description!)
            .WithName(category.Name)
            .Build();
    }
}