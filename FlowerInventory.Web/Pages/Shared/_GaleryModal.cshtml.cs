using FlowerInventorySystem.FlowerInventory.Web.Dto;

namespace FlowerInventorySystem.Pages.Media;

public class GalleryModal
{
    public string ModalId { get; init; } = "galleryModal";
    public string Title { get; init; } = "Select image";
    public List<StorageObject> Items { get; init; } = [];
}