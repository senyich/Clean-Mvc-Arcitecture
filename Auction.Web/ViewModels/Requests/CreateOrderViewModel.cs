using System.ComponentModel.DataAnnotations;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Web.ViewModels
{
    public class CreateOrderViewModel
    {
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();

        [Required(ErrorMessage = "Выберите предмет")]
        [Display(Name = "Предмет")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Укажите цену выкупа")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Цена выкупа ($)")]
        public decimal BuyPrice { get; set; }
    }
}