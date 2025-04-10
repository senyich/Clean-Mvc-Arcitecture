using Auction.Domain.Abstractions;
using Auction.Domain.Models;
using Auction.Domain.Enums;
using Auction.Web.ViewModels;
using Auction.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Web.Controllers
{
    public class MainController : Controller
    {
        private IAuctionValidationService auctionRepository;
        private IGameValidationService gameRepository;
        private ILoggerService logger;
        public MainController(
            IAuctionValidationService auctionRepository,
            IGameValidationService gameRepository,
            ILoggerService logger
            )
        {
            this.logger = logger;
            this.auctionRepository = auctionRepository;
            this.gameRepository = gameRepository;
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
        [Route("/CreateLot")]
        public async Task<IActionResult> CreateLot()
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
        [Route("/CreateGame")]
        public async Task<IActionResult> CreateGame()
        { 
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
            return View();
        }
    }
}