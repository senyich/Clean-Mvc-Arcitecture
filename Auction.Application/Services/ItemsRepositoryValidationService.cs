using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Application.Services
{
    public class ItemsRepositoryValidationService : IItemValidationService
    {        
        private IConverter<ItemEntity, ItemModel> itemEntityToModelConverter;
        private IConverter<ItemModel, ItemEntity> itemModelToEntityConverter;
        private IDbRepository<ItemEntity> itemDbRepository;
        private ILoggerService logger;
        public ItemsRepositoryValidationService(
            ILoggerService logger, 
            IDbRepository<ItemEntity> itemDbRepository,
            IConverter<ItemEntity, ItemModel> itemEntityToModelConverter,
            IConverter<ItemModel, ItemEntity> itemModelToEntityConverter)
        {
            this.logger = logger;
            this.itemDbRepository = itemDbRepository;
            this.itemEntityToModelConverter = itemEntityToModelConverter;
            this.itemModelToEntityConverter = itemModelToEntityConverter;
        }
        public async Task<int> AddItemAsync(ItemModel game)
        {
            try
            {
                var itemEntity = await itemModelToEntityConverter.ConvertAsync(game);
                if (itemEntity == null)
                    throw new ArgumentNullException();
                int id = await itemDbRepository.Add(itemEntity);
                await logger.LogAsync("ItemValidService", $"данные о предмете №{itemEntity.Id} были занесены успешно", LogType.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"добавление данных - {ex.Message}", LogType.Error);
                return -1;
            }
        }
        public async Task RemoveItemAsync(int id)
        {
            try
            {
                await itemDbRepository.Delete(id);
                await logger.LogAsync("ItemValidService", $"предмет №{id} был удален успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"удаление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task UpdateItemAsync(int id, ItemModel game)
        {
            try
            {
                var itemEntity = await itemModelToEntityConverter.ConvertAsync(game);
                await itemDbRepository.Update(id, itemEntity);
                await logger.LogAsync("ItemValidService", $"предмет №{id} был обновлен успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"обновление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task<ItemModel?> GetSingleItemAsync(int id)
        {
            try
            {
                var item = await itemDbRepository.Get(id);
                if (item == null)
                    throw new ArgumentNullException();
                await logger.LogAsync("ItemValidService", $"предмет №{id} был получен удачно", LogType.Success);
                return await itemEntityToModelConverter.ConvertAsync(item);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"получение данных - {ex.Message}", LogType.Error);
                return null;
            }
        }
        public async Task<List<ItemModel>?> GetAllItemsAsync()
        {
            try
            {
                var items = await itemDbRepository.GetAll();
                await logger.LogAsync("ItemValidService", $"все предметы были получены успешно", LogType.Success);
                return items.Select(async item => await itemEntityToModelConverter.ConvertAsync(item))
                    .Select(t=>t.Result)
                    .ToList();
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"получение всех данных - {ex.Message}", LogType.Error);
                return null;
            }
        }
    }

}

