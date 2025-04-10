using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class GameEntityConverterService : IConverter<GameEntity, GameModel>
    {
        private ILoggerService logger;
        public GameEntityConverterService(ILoggerService logger)
        {
            this.logger = logger;
        }
        public async Task<GameModel> Convert(GameEntity obj)
        {
            var game = GameModel.Create(obj.Id, obj.Name, obj.Description, obj.ImgPath, obj.AuctionId);
            if(game.model!=null)
            {
                return game.model;
            }
            else
            {
                throw new ArgumentNullException(game.error);
            }
        }
    }
}