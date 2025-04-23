using Microsoft.AspNetCore.Mvc;
using OrderWebsite.Application.Abstractions;
using OrderWebsite.Web.ViewModels;
using System.Security.Claims;

namespace OrderWebsite.Web.Controllers
{
    public class MainController : Controller
    {
        private IOrderValidationService auctionValidator;
        private IItemValidationService itemValidator;
        private IUserValidationService userValidator;
        public MainController(
            IOrderValidationService auctionValidator,
            IItemValidationService itemValidator,
            IUserValidationService userValidator
            )
        {
            this.auctionValidator = auctionValidator;
            this.itemValidator = itemValidator;
            this.userValidator = userValidator;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var games = await itemValidator.GetAllItemsAsync();
            var auctions = await auctionValidator.GetAllOrdersAsync();
            var users = await userValidator.GetAllUsersAsync();
            if (games == null && auctions == null && users == null)
                return View(new OrdersAndItemsViewModel());
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var view = new OrdersAndItemsViewModel()
                {
                    UserId = 0,
                    Orders = auctions,
                    Items = games,
                    Users = users
                };
                return View(view);
            }
            else
            {
                if (games != null && auctions != null && users != null)
                {
                    var view = new OrdersAndItemsViewModel()
                    {
                        UserId = int.Parse(userId),
                        Orders = auctions,
                        Items = games,
                        Users = users
                    };
                    return View(view);
                }
                else
                    return View(new OrdersAndItemsViewModel());
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CreateOrder()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            var games = await itemValidator.GetAllItemsAsync();
            var auctions = await auctionValidator.GetAllOrdersAsync();
            var view = new CreateOrderViewModel()
            {
                Items = games.Where(i=>i.OwnerId == int.Parse(userId)).ToList()
            };
            return View(view);
        } 
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CreateNewItem()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            return View();
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items()
        {
            var games = await itemValidator.GetAllItemsAsync();
            var users = await userValidator.GetAllUsersAsync();
            if (users != null && games != null)
            {
                var view = new AllItemsViewModel()
                {
                    Items = games,
                    Users = users
                };
                return View(view);
            }
            else
                return View(new AllItemsViewModel());
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Authorization()
        {
            var usrModel = new UserAuthViewModel();
            return View(usrModel);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> UserCabinet()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            var user = await userValidator.GetSingleUserAsync(int.Parse(userId));
            var items = await itemValidator.GetAllItemsAsync();
            if (items != null && user != null)
            {
                var userViewModel = new UserDataViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Balance = user.Balance,
                    Items = items.Where(u=>u.Id == int.Parse(userId)).ToList()
                };
                return View(userViewModel);
            }
            else
                return View(new UserDataViewModel());
        }
    }
}