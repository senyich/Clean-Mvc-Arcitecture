using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Abstractions
{
    public interface IItemValidationService
    {
        Task<int> CreateItemAsync(ItemModel item);
        Task<List<ItemModel>> GetAllItemsAsync();
        Task<ItemModel?> GetSingleItemAsync(int id);
        Task RemoveItemAsync(int id);
        Task UpdateItemAsync(int id, ItemModel item);
    }
}