using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class OrderEntityToModelConverterService : IConverter<OrderEntity, OrderModel>
    {
        public async Task<OrderModel> ConvertAsync(OrderEntity orderEntity)
        {
            (OrderModel model, string error) orderModel = OrderModel.Create(orderEntity.Id, orderEntity.ItemId, orderEntity.OwnerId, orderEntity.BuyPrice);
            if(orderModel.model!=null)
                return orderModel.model;
            else
                throw new ArgumentNullException(orderModel.error);
        }
    }
} 