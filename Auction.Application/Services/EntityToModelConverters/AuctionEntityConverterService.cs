using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;
using Auction.Domain.Enums;

namespace Auction.Application.Services
{
    public class AuctionEntityConverterService : IConverter<AuctionEntity, AuctionModel>
    {
        private ILoggerService logger;
        public AuctionEntityConverterService(ILoggerService logger)
        {
            this.logger = logger;
        }
        public async Task<AuctionModel> Convert(AuctionEntity obj)
        {
            var auction = AuctionModel.Create(obj.Id, obj.GameId, obj.CurrentPrice, obj.BuyPrice, obj.MinPriceUpdateRate);
            if(auction.model!=null)
            {
                return auction.model;
            }
            else
            {
                throw new ArgumentNullException(auction.error);
            }
        }
    }
} 