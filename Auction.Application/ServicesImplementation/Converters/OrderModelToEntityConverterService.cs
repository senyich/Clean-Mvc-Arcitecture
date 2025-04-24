using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class OrderModelToEntityConverterService : IConverterService<OrderModel, OrderEntity>
    {
        public async Task<OrderEntity> ConvertAsync(OrderModel orderModel)
        {
            var orderEntity = new OrderEntity()
            {
                BuyPrice = orderModel.BuyPrice,
                ItemId = orderModel.ItemId,
                OwnerId = orderModel.OwnerId
            };
            orderEntity.BuyPrice = orderModel.BuyPrice;
            orderEntity.ItemId = orderModel.ItemId;
            orderEntity.OwnerId = orderModel.OwnerId;
            return orderEntity;
        }
    }
}