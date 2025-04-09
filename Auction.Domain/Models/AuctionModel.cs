using System.Text;

namespace Auction.Domain.Models
{
    public class AuctionModel
    {
        private AuctionModel(int id,int gameId,decimal currentPrice, decimal buyPrice, decimal minPriceUpdateRate)
        {
            Id = id;
            GameId = gameId;
            CurrentPrice = currentPrice;
            BuyPrice = buyPrice;
            MinPriceUpdateRate = minPriceUpdateRate;
        }
        public int Id {get;}
        public int GameId {get;}
        public decimal CurrentPrice{get;}
        public decimal BuyPrice {get;}
        public decimal MinPriceUpdateRate {get;}
        public static (AuctionModel model, string error) Create(int id,int gameId,decimal currentPrice, decimal buyPrice, decimal minPriceUpdateRate)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if(currentPrice <= 0 || buyPrice <= 0 || minPriceUpdateRate <= 0)
            {
                errorBuilder.Append("Недопустимые цены!");
                return (null, errorBuilder.ToString())!;
            }
            AuctionModel auctionModel = new AuctionModel(id,gameId, currentPrice, buyPrice, minPriceUpdateRate);
            return (auctionModel, string.Empty);
        }
    }

}

