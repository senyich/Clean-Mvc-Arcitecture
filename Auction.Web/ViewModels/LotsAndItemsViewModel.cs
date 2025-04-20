using System.ComponentModel.DataAnnotations;
using Auction.Domain.Models;

namespace Auction.Web.ViewModels
{
    public class LotsAndItemsViewModel
    {
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
        public List<AuctionModel> Auctions { get; set; } = new List<AuctionModel>();
        public List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}