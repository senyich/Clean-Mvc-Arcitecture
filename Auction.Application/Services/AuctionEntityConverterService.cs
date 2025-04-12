using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class AuctionEntityConverterService : IConverter<AuctionEntity, AuctionModel>
    {
        public async Task<AuctionModel> ConvertAsync(AuctionEntity obj)
        {
            var auction = AuctionModel.Create(obj.Id, obj.ItemId, obj.CurrentPrice, obj.BuyPrice, obj.MinPriceUpdateRate);
            if(auction.model!=null)
                return auction.model;
            else
                throw new ArgumentNullException(auction.error);
        }
    }
} 