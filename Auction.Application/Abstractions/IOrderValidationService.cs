using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Abstractions
{
    public interface IOrderValidationService
    {
        Task<int> CreateOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel?> GetSingleOrderAsync(int id);
        Task RemoveOrderAsync(int id);
        Task UpdateOrderAsync(int id, OrderModel newOrder);
    }
}