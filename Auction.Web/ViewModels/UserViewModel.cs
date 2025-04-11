using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Auction.Domain.Models;

namespace Auction.Web.ViewModels
{
    public class UserAuthViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [Display(Name = "Имя пользователя")]
        [RegularExpression(@"^@[a-zA-Z0-9_]+$", ErrorMessage = "Имя должно начинаться с @ и содержать только буквы, цифры и подчеркивания")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 20 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        
        [DefaultValue(true)]
        public bool IsLoginMode { get; set; } = true;
        public List<ItemModel> Games {get;set;} = new List<ItemModel>();
    }
}