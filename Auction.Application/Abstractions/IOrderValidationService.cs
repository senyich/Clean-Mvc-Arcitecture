using Auction.Domain.Models;

namespace Auction.Application.Abstractions
{
    public interface IAuctionValidationService
    {
        Task<int> CreateOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel?> GetSingleOrderAsync(int id);
        Task RemoveOrderAsync(int id);
        Task UpdateOrderAsync(int id, OrderModel newOrder);
    }
}