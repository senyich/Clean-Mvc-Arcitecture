using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Application.Abstractions.IValidators;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Enums;
using OrderWebsite.Domain.Models;
using OrderWebsite.Domain.Repositories;
using OrderWebsite.Domain.Repositories.Abstraction;

namespace OrderWebsite.Application.Services
{
    public class ItemsRepositoryValidationService : IItemValidationService
    {        
        private IConverterService<ItemEntity, ItemModel> itemEntityToModelConverter;
        private IConverterService<ItemModel, ItemEntity> itemModelToEntityConverter;
        private IItemRepository itemRepository;
        private ILoggerService logger;
        public ItemsRepositoryValidationService(
            ILoggerService logger,
            IItemRepository itemRepository,
            IConverterService<ItemEntity, ItemModel> itemEntityToModelConverter,
            IConverterService<ItemModel, ItemEntity> itemModelToEntityConverter)
        {
            this.logger = logger;
            this.itemRepository = itemRepository;
            this.itemEntityToModelConverter = itemEntityToModelConverter;
            this.itemModelToEntityConverter = itemModelToEntityConverter;
        }
        public async Task<int> CreateItemAsync(ItemModel item)
        {
            try
            {
                var itemEntity = await itemModelToEntityConverter.ConvertAsync(item);
                int id = await itemRepository.Add(itemEntity);
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
                await itemRepository.Delete(id);
                await logger.LogAsync("ItemValidService", $"предмет №{id} был удален успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"удаление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task UpdateItemAsync(int id, ItemModel item)
        {
            try
            {
                var itemEntity = await itemModelToEntityConverter.ConvertAsync(item);
                await itemRepository.Update(id, itemEntity);
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
                var item = await itemRepository.Get(id);
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
                var items = await itemRepository.GetAll();
                await logger.LogAsync("ItemValidService", $"все предметы были получены успешно", LogType.Success);
                return items
                    .Select(async item => await itemEntityToModelConverter.ConvertAsync(item))
                    .Select(t=>t.Result)
                    .ToList();
            }
            catch (Exception ex)
            {
                await logger.LogAsync("ItemValidService", $"получение всех данных - {ex.Message}", LogType.Error);
                return new List<ItemModel>();
            }
        }
    }

}

