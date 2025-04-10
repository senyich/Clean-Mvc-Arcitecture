using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class GameRepositoryValidationService : IGameValidationService
    {        
        private IConverter<GameEntity, GameModel> entityConverter;
        private IConverter<GameModel, GameEntity> modelConverter;
        private IDbRepository<GameEntity> dbRepository;
        private ILoggerService logger;
        public GameRepositoryValidationService(
            ILoggerService logger, 
            IDbRepository<GameEntity> dbRepository,
            IConverter<GameEntity, GameModel> entityConverter,
            IConverter<GameModel, GameEntity> modelConverter)
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.entityConverter = entityConverter;
            this.modelConverter = modelConverter;
        }
        public async Task<int> AddGameAsync(GameModel game)
        {
            try
            {
                var gameEntity = await modelConverter.Convert(game); 
                int id = await dbRepository.Add(gameEntity);
                await logger.LogAsync("GameValidation", $"Данные об игре  №{gameEntity.Id} были занесены успешно", LogState.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("GameValidation", $"add data - {ex.Message}", LogState.Error);
                return -1;
            }
        }
        public async Task RemoveGameAsync(int id)
        {
            try
            {
                await dbRepository.Delete(id);
                await logger.LogAsync("GameValidation", $"игра №{id} была удалена успешно", LogState.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("GameValidation", $"rm data - {ex.Message}", LogState.Error);
            }
        }
        public async Task UpdateGameAsync(int id, GameModel game)
        {
            try
            {
                var gameEntity = await modelConverter.Convert(game);
                await dbRepository.Update(id, gameEntity);
                await logger.LogAsync("GameValidation", $"игра №{id} была обновлена успешно", LogState.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("GameValidation", $"upd data - {ex.Message}", LogState.Error);
            }
        }
        public async Task<GameModel> GetGameAsync(int id)
        {
            try
            {
                var game = await dbRepository.Get(id);
                await logger.LogAsync("GameValidation", $"игра №{id} была получена успешно", LogState.Success);
                return await entityConverter.Convert(game);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("GameValidation", $"get single data - {ex.Message}", LogState.Error);
                return null;
            }
        }
        public async Task<List<GameModel>> GetAllGamesAsync()
        {
            try
            {
                var games = await dbRepository.GetAll();
                await logger.LogAsync("GameValidation", $"все игры были получены успешно", LogState.Success);
                return games.Select(async g => await entityConverter.Convert(g))
                    .Select(t=>t.Result)
                    .ToList();//с ToListAsync() не сработало
            }
            catch (Exception ex)
            {
                await logger.LogAsync("GameValidation", $"get all data - {ex.Message}", LogState.Error);
                return null;
            }
        }
    }

}

