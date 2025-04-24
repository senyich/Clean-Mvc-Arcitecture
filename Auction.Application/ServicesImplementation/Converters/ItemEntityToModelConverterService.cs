using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class ItemEntityToModelConverterService : IConverterService<ItemEntity, ItemModel>
    {
        public async Task<ItemModel> ConvertAsync(ItemEntity itemEntity)
        {
            (ItemModel model, string error) item = ItemModel.Create(
                itemEntity.Id, 
                itemEntity.Name, 
                itemEntity.Description,
                itemEntity.ImgPath, 
                itemEntity.OrderId, 
                itemEntity.OwnerId);
            if(item.model!=null)
                return item.model;
            else
                throw new ArgumentNullException(item.error);
        }
    }
}