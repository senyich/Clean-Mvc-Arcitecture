using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Auction.Domain.Models;

namespace Auction.Web.ViewModels
{
    public class UserDataViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public decimal Balance { get; set; } 
        public List<ItemModel> Items {get;set;} = new List<ItemModel>();
    }
}