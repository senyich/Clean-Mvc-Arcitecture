
namespace OrderWebsite.Domain.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; } 
        public int OwnerId { get; set; }
        public UserEntity Owner { get; set; } 
        public decimal BuyPrice { get; set; }
    }
}

