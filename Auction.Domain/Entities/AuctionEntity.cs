namespace Auction.Domain.Entities
{
    public class AuctionEntity
    {
        public int Id {get;set;}
        public int GameId {get;set;}
        public GameEntity Game {get;set;}
        public decimal CurrentPrice {get;set;}
        public decimal BuyPrice {get;set;}
        public decimal MinPriceUpdateRate {get;set;}
    }
}

