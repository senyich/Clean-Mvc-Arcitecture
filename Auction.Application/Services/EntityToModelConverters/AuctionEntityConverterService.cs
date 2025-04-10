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
                await logger.LogAsync("AuctionEntityConverter","Успешная конвертация",LogState.Success);
                return auction.model;
            }
            else
            {
                await logger.LogAsync("AuctionEntityConverter",$"Ошибка конвертации: {auction.error}",LogState.Error);
                throw new ArgumentNullException(auction.error);
            }
        }
    }
} 