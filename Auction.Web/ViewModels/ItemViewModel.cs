using System.ComponentModel.DataAnnotations;

namespace Auction.Web.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Название предмета обязательно")]
        [StringLength(50, ErrorMessage = "Название не должно превышать 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Изображение")]
        public IFormFile ImageFile { get; set; }
        public string ExistingImagePath { get; set; }
    }
}