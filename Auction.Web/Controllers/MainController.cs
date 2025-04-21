using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Models;
using OrderWebsite.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OrderWebsite.Web.Controllers
{
    public class MainController : Controller
    {
        private IOrderValidationService auctionRepository;
        private IItemValidationService itemRepository;
        private IUserValidationService userRepository;
        public MainController(
            IOrderValidationService auctionRepository,
            IItemValidationService gameRepository,
            IUserValidationService userRepository
            )
        {
            this.auctionRepository = auctionRepository;
            this.itemRepository = gameRepository;
            this.userRepository = userRepository;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var games = await itemRepository.GetAllItemsAsync();
            var auctions = await auctionRepository.GetAllOrdersAsync();
            var users = await userRepository.GetAllUsersAsync();
            if (games != null && auctions!=null && users!=null)
            {
                var view = new OrdersAndItemsViewModel()
                {
                    Orders = auctions,
                    Items = games,
                    Users = users
                };
                return View(view);
            }
            else 
                return View(new OrdersAndItemsViewModel());
        }
        [HttpGet]
        [Route("/CreateLot")]
        public async Task<IActionResult> CreateLot()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            var games = await itemRepository.GetAllItemsAsync();
            var auctions = await auctionRepository.GetAllOrdersAsync();
            var view = new CreateOrderViewModel()
            {
                Items = games.Where(i=>i.OwnerId == int.Parse(userId)).ToList()
            };
            return View(view);
        } 
        [HttpGet]
        [Route("/CreateNewItem")]
        public async Task<IActionResult> CreateNewItem()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            return View();
        }
        [HttpGet]
        [Route("/Items")] 
        public async Task<IActionResult> Items()
        {
            var games = await itemRepository.GetAllItemsAsync();
            var users = await userRepository.GetAllUsersAsync();
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
        [Route("/Authorization")]
        public async Task<IActionResult> Authorization()
        {
            var usrModel = new UserAuthViewModel();
            return View(usrModel);
        }
        [HttpGet]
        [Route("/UserCabinet")]
        public async Task<IActionResult> UserCabinet()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            var user = await userRepository.GetSingleUserAsync(int.Parse(userId));
            var items = await itemRepository.GetAllItemsAsync();
            if (items != null && user != null)
            {
                var userViewModel = new UserDataViewModel()
                {
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