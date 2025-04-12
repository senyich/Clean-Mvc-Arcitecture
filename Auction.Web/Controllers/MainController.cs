using Auction.Application.Abstractions;
using Auction.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auction.Web.Controllers
{
    public class MainController : Controller
    {
        private IAuctionValidationService auctionRepository;
        private IItemValidationService itemRepository;
        private IUserValidationService userRepository;
        public MainController(
            IAuctionValidationService auctionRepository,
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
            var auctions = await auctionRepository.GetAllLotsAsync();
            var view = new AuctionLotsViewModel()
            {
                Auctions = auctions,
                Items = games
            };
            return View(view);
        }
        [HttpGet]
        [Route("/CreateLot")]
        public async Task<IActionResult> CreateLot()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            var games = await itemRepository.GetAllItemsAsync();
            var auctions = await auctionRepository.GetAllLotsAsync();
            var view = new AuctionLotsViewModel()
            {
                Auctions = auctions,
                Items = games
            };
            return View(view);
        } 
        [HttpGet]
        [Route("/AddItem")]
        public async Task<IActionResult> AddItem()
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
            var view = new ItemsViewModel()
            {
                Items = games
            };
            return View(view);
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
            var userViewModel = new UserAuthViewModel()
            {
                UserName = user.UserName
            };
            return View(userViewModel);
        }
    }
}