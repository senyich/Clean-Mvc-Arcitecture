using Auction.Domain.Models;

namespace Auction.Application.Abstractions
{
    public interface IItemValidationService
    {
        Task<int> AddItemAsync(ItemModel game);
        Task<List<ItemModel>> GetAllItemsAsync();
        Task<ItemModel> GetSingleItemAsync(int id);
        Task RemoveItemAsync(int id);
        Task UpdateItemAsync(int id, ItemModel game);
    }
}