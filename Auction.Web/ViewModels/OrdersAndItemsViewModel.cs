using System.ComponentModel.DataAnnotations;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Web.ViewModels
{
    public class OrdersAndItemsViewModel
    {
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}