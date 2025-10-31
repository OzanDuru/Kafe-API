namespace Kafe.Application.Dtos.MenuItemDtos
{
    public class UpdateMenuItemDto
    {
        public int Id { get; set; } // Hangi menü öğesinin güncelleneceğini belirtmek için Id ekledik
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

    }
} 