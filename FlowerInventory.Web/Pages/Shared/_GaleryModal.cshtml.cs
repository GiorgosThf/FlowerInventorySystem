using FlowerInventory.Web.Dto;

namespace FlowerInventory.Web.Pages.Shared;

public class GalleryModal
{
    public string ModalId { get; init; } = "galleryModal";
    public string Title { get; init; } = "Select image";
    public List<StorageObject> Items { get; init; } = [];
}