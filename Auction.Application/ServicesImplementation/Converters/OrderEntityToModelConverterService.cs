using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class OrderEntityToModelConverterService : IConverterService<OrderEntity, OrderModel>
    {
        public async Task<OrderModel> ConvertAsync(OrderEntity orderEntity)
        {
            (OrderModel model, string error) order = OrderModel.Create(
                orderEntity.Id,
                orderEntity.ItemId,
                orderEntity.OwnerId,
                orderEntity.BuyPrice);
            if(order.model!=null)
                return order.model;
            else
                throw new ArgumentNullException(order.error);
        }
    }
} 