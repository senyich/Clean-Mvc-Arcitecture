using Auction.Domain.Abstractions;
using Auction.Domain.Models;
using Auction.Domain.Enums;
using Auction.Web.ViewModels;
using Auction.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Auction.Web.Controllers
{
    public class MainController : Controller
    {
        private IAuctionValidationService auctionRepository;
        private IGameValidationService gameRepository;
        private IUserValidationService userRepository;
        private ILoggerService logger;
        public MainController(
            IAuctionValidationService auctionRepository,
            IGameValidationService gameRepository,
            ILoggerService logger,
            IUserValidationService userRepository
            )
        {
            this.logger = logger;
            this.auctionRepository = auctionRepository;
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var games = await gameRepository.GetAllGamesAsync();
            var auctions = await auctionRepository.GetAllLotsAsync();
            var view = new AuctionLotsViewModel()
            {
                Auctions = auctions,
                Games = games
            };
            return View(view);
        }
        [HttpGet]
        [Authorize]
        [Route("/CreateLot")]
        public async Task<IActionResult> CreateLot()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            var games = await gameRepository.GetAllGamesAsync();
            var auctions = await auctionRepository.GetAllLotsAsync();
            var view = new AuctionLotsViewModel()
            {
                Auctions = auctions,
                Games = games
            };
            return View(view);
        } 
        [HttpGet]
        [Route("/CreateGame")]
        public async Task<IActionResult> CreateGame()
        { 
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");
            return View();
        }
        [HttpGet]
        [Route("/Games")]
        public async Task<IActionResult> Games()
        {
            var games = await gameRepository.GetAllGamesAsync();
            var view = new AllGamesViewModel()
            {
                Games = games
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
            var user = await userRepository.GetUserAsync(int.Parse(userId));
            var userViewModel = new UserAuthViewModel()
            {
                UserName = user.UserName
            };
            return View(userViewModel);
        }
    }
}