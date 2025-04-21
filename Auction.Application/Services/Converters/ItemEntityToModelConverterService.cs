using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class ItemEntityToModelConverterService : IConverter<ItemEntity, ItemModel>
    {
        public async Task<ItemModel> ConvertAsync(ItemEntity itemEntity)
        {
            (ItemModel model, string error) itemModel = ItemModel.Create(itemEntity.Id, itemEntity.Name, itemEntity.Description, itemEntity.ImgPath, itemEntity.OrderId, itemEntity.OwnerId);
            if(itemModel.model!=null)
                return itemModel.model;
            else
                throw new ArgumentNullException(itemModel.error);
        }
    }
}