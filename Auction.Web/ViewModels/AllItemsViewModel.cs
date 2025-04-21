using OrderWebsite.Domain.Models;

namespace OrderWebsite.Web.ViewModels
{
    public class AllItemsViewModel
    {
        public List<ItemModel> Items {get;set;} = new List<ItemModel>();
        public List<UserModel> Users { get;set;} = new List<UserModel>();
    }
}