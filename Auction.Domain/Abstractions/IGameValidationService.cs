using Auction.Domain.Models;

namespace Auction.Domain.Abstractions
{
    public interface IGameValidationService
    {
        Task<int> AddGameAsync(GameModel game);
        Task<List<GameModel>> GetAllGamesAsync();
        Task<GameModel> GetGameAsync(int id);
        Task RemoveGameAsync(int id);
        Task UpdateGameAsync(int id, GameModel game);
    }
}