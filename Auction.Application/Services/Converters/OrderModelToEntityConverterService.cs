using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class OrderModelToEntityConverterService : IConverter<OrderModel, OrderEntity>
    {
        public async Task<OrderEntity> ConvertAsync(OrderModel orderModel)
        {
            var orderEntity = new OrderEntity();
            orderEntity.BuyPrice = orderModel.BuyPrice;
            orderEntity.CurrentPrice = orderModel.CurrentPrice;
            orderEntity.ItemId = orderModel.ItemId;
            orderEntity.MinPriceUpdateRate = orderModel.MinPriceUpdateRate;
            orderEntity.OwnerId = orderModel.OwnerId;
            return orderEntity;
        }
    }
}