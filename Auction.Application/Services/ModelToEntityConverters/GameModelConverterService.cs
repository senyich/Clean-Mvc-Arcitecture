using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class GameModelConverterService : IConverter<GameModel, GameEntity>
    {
        public async Task<GameEntity> Convert(GameModel model)
        {
            var game = new GameEntity();
            game.AuctionId = model.AuctionId;
            game.Description = model.Description;
            game.ImgPath = model.ImgPath;
            game.Name = model.Name;
            return game;
        }
    }
}
