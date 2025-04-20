using Auction.Domain.Models;

namespace Auction.Web.ViewModels
{
    public class AllItemsViewModel
    {
        public List<ItemModel> Items {get;set;} = new List<ItemModel>();
        public List<UserModel> Users { get;set;} = new List<UserModel>();
    }
}