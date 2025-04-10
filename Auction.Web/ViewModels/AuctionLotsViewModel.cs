
using System.ComponentModel.DataAnnotations;
using Auction.Domain.Models;

namespace Auction.Web.ViewModels
{
    public class AuctionLotsViewModel
    {
         public List<GameModel> Games { get; set; }
        public List<AuctionModel> Auctions { get; set; }

        [Required(ErrorMessage = "Выберите игру")]
        [Display(Name = "Игра")]
        public int GameId { get; set; }

        [Required(ErrorMessage = "Укажите текущую цену")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Текущая цена ($)")]
        public decimal CurrentPrice { get; set; }

        [Required(ErrorMessage = "Укажите цену выкупа")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Цена выкупа ($)")]
        public decimal BuyPrice { get; set; }

        [Required(ErrorMessage = "Укажите минимальный шаг ставки")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Шаг должен быть больше 0")]
        [Display(Name = "Мин. шаг ставки ($)")]
        public decimal MinPriceUpdateRate { get; set; }
    }
}