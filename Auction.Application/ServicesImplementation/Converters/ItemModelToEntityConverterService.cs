using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class ItemModelToEntityConverterService : IConverterService<ItemModel, ItemEntity>
    {
        public async Task<ItemEntity> ConvertAsync(ItemModel itemModel)
        {
            var itemEntity = new ItemEntity()
            {
                OrderId = itemModel.OrderId,
                Description = itemModel.Description,
                ImgPath = itemModel.ImgPath,
                Name = itemModel.Name,
                OwnerId = itemModel.OwnerId,
            };
            return itemEntity;
        }
    }
}
