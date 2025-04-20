using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class ItemEntityToModelConverterService : IConverter<ItemEntity, ItemModel>
    {
        public async Task<ItemModel> ConvertAsync(ItemEntity itemEntity)
        {
            (ItemModel model, string error) itemModel = ItemModel.Create(itemEntity.Id, itemEntity.Name, itemEntity.Description, itemEntity.ImgPath, itemEntity.AuctionId, itemEntity.OwnerId);
            if(itemModel.model!=null)
                return itemModel.model;
            else
                throw new ArgumentNullException(itemModel.error);
        }
    }
}