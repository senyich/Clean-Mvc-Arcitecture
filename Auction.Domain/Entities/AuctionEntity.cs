namespace Auction.Domain.Entities
{
    public class AuctionEntity
    {
        public int Id {get;set;}
        public int ItemId {get;set;}
        public ItemEntity Item {get;set;}
        public decimal CurrentPrice {get;set;}
        public decimal BuyPrice {get;set;}
        public decimal MinPriceUpdateRate {get;set;}
    }
}

