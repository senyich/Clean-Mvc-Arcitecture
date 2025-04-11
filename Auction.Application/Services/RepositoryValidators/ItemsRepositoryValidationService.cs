using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Application.Services
{
    public class ItemsRepositoryValidationService : IItemValidationService
    {        
        private IConverter<ItemEntity, ItemModel> entityConverter;
        private IConverter<ItemModel, ItemEntity> modelConverter;
        private IDbRepository<ItemEntity> dbRepository;
        private ILoggerRepository logger;
        public ItemsRepositoryValidationService(
            ILoggerRepository logger, 
            IDbRepository<ItemEntity> dbRepository,
            IConverter<ItemEntity, ItemModel> entityConverter,
            IConverter<ItemModel, ItemEntity> modelConverter)
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.entityConverter = entityConverter;
            this.modelConverter = modelConverter;
        }
        public async Task<int> AddItemAsync(ItemModel game)
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
        public async Task RemoveItemAsync(int id)
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
        public async Task UpdateItemAsync(int id, ItemModel game)
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
        public async Task<ItemModel> GetSingleItemAsync(int id)
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
        public async Task<List<ItemModel>> GetAllItemsAsync()
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

