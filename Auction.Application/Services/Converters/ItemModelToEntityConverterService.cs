using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class ItemModelToEntityConverterService : IConverter<ItemModel, ItemEntity>
    {
        public async Task<ItemEntity> ConvertAsync(ItemModel itemModel)
        {
            var itemEntity = new ItemEntity();
            itemEntity.AuctionId = itemModel.AuctionId;
            itemEntity.Description = itemModel.Description;
            itemEntity.ImgPath = itemModel.ImgPath;
            itemEntity.Name = itemModel.Name;
            itemEntity.OwnerId = itemModel.OwnerId;
            return itemEntity;
        }
    }
}
