using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class OrderEntityToModelConverterService : IConverter<OrderEntity, OrderModel>
    {
        public async Task<OrderModel> ConvertAsync(OrderEntity orderEntity)
        {
            (OrderModel model, string error) orderModel = OrderModel.Create(orderEntity.Id, orderEntity.ItemId, orderEntity.OwnerId, orderEntity.CurrentPrice, orderEntity.BuyPrice, orderEntity.MinPriceUpdateRate);
            if(orderModel.model!=null)
                return orderModel.model;
            else
                throw new ArgumentNullException(orderModel.error);
        }
    }
} 