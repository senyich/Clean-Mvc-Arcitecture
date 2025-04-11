using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class GameModelConverterService : IConverter<ItemModel, ItemEntity>
    {
        public async Task<ItemEntity> Convert(ItemModel model)
        {
            var item = new ItemEntity();
            item.AuctionId = model.AuctionId;
            item.Description = model.Description;
            item.ImgPath = model.ImgPath;
            item.Name = model.Name;
            return item;
        }
    }
}
