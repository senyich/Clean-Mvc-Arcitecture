
namespace OrderWebsite.Domain.Entities
{
    public class ItemEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; } 
        public int OwnerId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
    }
}
