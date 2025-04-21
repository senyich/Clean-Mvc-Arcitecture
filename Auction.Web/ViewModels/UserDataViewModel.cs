using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Web.ViewModels
{
    public class UserDataViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public decimal Balance { get; set; } 
        public List<ItemModel> Items {get;set;} = new List<ItemModel>();
    }
}