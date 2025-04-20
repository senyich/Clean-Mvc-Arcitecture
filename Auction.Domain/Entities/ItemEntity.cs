namespace Auction.Domain.Entities
{    
    public class ItemEntity
    {
        public int Id {get;set;}
        public int AuctionId {get;set;}
        public OrderEntity AuctionLot {get;set;}
        public int OwnerId { get; set;}
        public UserEntity Owner { get; set; }
        public string Name {get;set;}
        public string Description {get;set;}
        public string ImgPath {get;set;}
    }
}
