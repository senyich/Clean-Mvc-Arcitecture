using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class ItemEntityConverterService : IConverter<ItemEntity, ItemModel>
    {
        public ItemEntityConverterService()
        {
        }
        public async Task<ItemModel> Convert(ItemEntity obj)
        {
            var game = ItemModel.Create(obj.Id, obj.Name, obj.Description, obj.ImgPath, obj.AuctionId);
            if(game.model!=null)
            {
                return game.model;
            }
            else
            {
                throw new ArgumentNullException(game.error);
            }
        }
    }
}