using Microsoft.AspNetCore.Mvc;
using Auction.Web.ViewModels;
using Auction.Domain.Models;
using Auction.Domain.Enums;
using Auction.Application.Abstractions;

namespace Auction.Web.Controllers
{
    public class FormController : Controller
    {
        private IAuctionValidationService auctionRepository;
        private IAuthService authService;
        private IItemValidationService itemRepository;
        private ILoggerService logger;
        private IFileLogisticService fileLogisticService;
        private IWebHostEnvironment environment;
        public FormController(
            IAuctionValidationService auctionRepository,
            ILoggerService logger,
            IFileLogisticService fileLogisticService,
            IWebHostEnvironment environment,
            IItemValidationService itemRepository,
            IAuthService authService
            )
        {
            this.itemRepository = itemRepository;
            this.auctionRepository = auctionRepository;
            this.logger = logger;
            this.fileLogisticService = fileLogisticService;
            this.environment = environment;
            this.authService = authService;
        }
        [HttpPost]
        [Route("/Main/AddAuctionLot")]
        public async Task<IActionResult> AddAuctionLot(AuctionLotsViewModel model)
        {
            var (auctionModel, error) = AuctionModel.Create(0, model.ItemId, model.CurrentPrice, model.BuyPrice,model.MinPriceUpdateRate);    
            if(string.IsNullOrEmpty(error))
            {          
                int id = await auctionRepository.AddAuctionLotAsync(auctionModel);

                var tmpItem = await itemRepository.GetSingleItemAsync(model.ItemId);

                var (updGame, gameError) = ItemModel.Create(tmpItem.Id, tmpItem.Name, tmpItem.Description, tmpItem.ImgPath, id);
                
                await itemRepository.UpdateItemAsync(model.ItemId, updGame);
                await logger.LogAsync("FormController", "успешное добавление лота", LogType.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController",error,LogType.Error);
            return RedirectToAction("Index", "Main");
        }
        [HttpPost]
        [Route("/Main/AddItem")]
        public async Task<IActionResult> AddItem(ItemViewModel game)
        {
            string filePath = string.Empty;
            try
            {
                filePath = await fileLogisticService.SaveFileAsync(game.ImageFile, environment.WebRootPath);
                await logger.LogAsync("FormController","успешное добавление файла", LogType.Success);
            }
            catch(Exception ex)
            {
                await logger.LogAsync("FormController",$"ошибка при добавлении файла: {ex.Message}", LogType.Error);
            }
            var (itemModel, error)  = ItemModel.Create(
                id: 0,
                name: game.Name,
                description: game.Description,
                imgPath: filePath,
                auctionId: 0
            );
            if(string.IsNullOrEmpty(error))
            {
                await itemRepository.AddItemAsync(itemModel);
                await logger.LogAsync("FormController", "успешное добавление предмета", LogType.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController", $"ошибка добавления предмета - {error}", LogType.Error);
            return RedirectToAction("Items", "Main");
        }

        [HttpPost]
        [Route("/Main/Register")]
        public async Task<IActionResult> RegisterUser(UserAuthViewModel userFormData)
        {
            await authService.RegisterAsync(userFormData.UserName, userFormData.Password);
            return await LoginUser(userFormData);
        }
   
        [HttpPost]
        [Route("/Main/Login")]
        public async Task<IActionResult> LoginUser(UserAuthViewModel userFormData)
        {
            try
            {
                var jwtToken = await authService.LoginAsync(userFormData.UserName, userFormData.Password);
                HttpContext.Response.Cookies.Append("myToken", jwtToken);
                await logger.LogAsync("FormController", "успешный вход в систему/регистрация", LogType.Success);
                return RedirectToAction("Index", "Main");
            }
            catch(Exception ex)
            {
                await logger.LogAsync("FormController", $"ошибка регистрации/авторизации - {ex.Message}", LogType.Error);
                return Content(ex.Message);
            }
        }
    }
}