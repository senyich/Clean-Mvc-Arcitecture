namespace Auction.Domain.Entities
{    
    public class GameEntity
    {
        public int Id {get;set;}
        public int AuctionId {get;set;}
        public AuctionEntity AuctionLot {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public string ImgPath {get;set;}
    }
}
