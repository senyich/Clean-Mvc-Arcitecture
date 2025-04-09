using Microsoft.AspNetCore.Mvc;
using Auction.Web.ViewModels;
using Auction.Domain.Models;
using Auction.Domain.Enums;
using Auction.Domain.Abstractions;
using Auction.Domain.Entities;

namespace Auction.Web.Controllers
{
    public class FormController : Controller
    {
        private IAuctionValidationService auctionRepository;
        private IGameValidationService gameRepository;
        private ILoggerService logger;
        private IFileLogisticService fileLogisticService;
        private IWebHostEnvironment environment;
        private IConverter<GameModel,GameEntity> gameModelConverter;
        public FormController(
            IAuctionValidationService auctionRepository,
            ILoggerService logger,
            IFileLogisticService fileLogisticService,
            IWebHostEnvironment environment,
            IGameValidationService gameRepository
            )
        {
            this.gameRepository = gameRepository;
            this.auctionRepository = auctionRepository;
            this.logger = logger;
            this.fileLogisticService = fileLogisticService;
            this.environment = environment;
        }
        [HttpPost]
        [Route("/Home/AddAuctionLot")]
        public async Task<IActionResult> AddAuctionLot(AuctionLotsViewModel model)
        {
            var (auctionModel, error) = AuctionModel.Create(0, model.GameId, model.CurrentPrice, model.BuyPrice,model.MinPriceUpdateRate);    
            if(string.IsNullOrEmpty(error))
            {          
                int id = await auctionRepository.AddAuctionLotAsync(auctionModel);

                var tmpGame = await gameRepository.GetGameAsync(model.GameId);

                var (updGame, gameError) = GameModel.Create(tmpGame.Id, tmpGame.Name, tmpGame.Description, tmpGame.ImgPath, id);
                
                await gameRepository.UpdateGameAsync(model.GameId, updGame);
                await logger.LogAsync("FormController", "Успешное добавление лота", LogState.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController",error,LogState.Error);
            return RedirectToAction("Index", "Main");
        }
        [HttpPost]
        [Route("/Home/AddGame")]
        public async Task<IActionResult> AddGame(GameViewModel game)
        {
            string filePath = string.Empty;
            try
            {
                filePath = await fileLogisticService.SaveFileAsync(game.ImageFile, environment.WebRootPath);
                await logger.LogAsync("FormController","Успешное добавление файла", LogState.Success);
            }
            catch(Exception ex)
            {
                await logger.LogAsync("FormController",$"Ошибка при добавлении файла: {ex.Message}", LogState.Error);
            }
            var (gameModel, error)  = GameModel.Create(
                id: 0,
                name: game.Name,
                description: game.Description,
                imgPath: filePath,
                auctionId: 0
            );
            if(string.IsNullOrEmpty(error))
            {
                await gameRepository.AddGameAsync(gameModel);
                await logger.LogAsync("FormController", "Успешное добавление игры", LogState.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController",error,LogState.Error);
            return RedirectToAction("Index", "Main");
        }

    }
}