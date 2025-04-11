using Microsoft.AspNetCore.Mvc;
using Auction.Web.ViewModels;
using Auction.Domain.Models;
using Auction.Domain.Enums;
using Auction.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Auction.Application.Abstractions;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Web.Controllers
{
    public class FormController : Controller
    {
        private IAuctionValidationService auctionRepository;
        private ISecurityService securityService;
        private IAuthService authService;
        private IItemValidationService itemRepository;
        private ILoggerRepository logger;
        private IFileLogisticService fileLogisticService;
        private IWebHostEnvironment environment;
        private IConverter<ItemModel,ItemEntity> gameModelConverter;
        public FormController(
            IAuctionValidationService auctionRepository,
            ILoggerRepository logger,
            IFileLogisticService fileLogisticService,
            IWebHostEnvironment environment,
            IItemValidationService itemRepository,
            ISecurityService securityService,
            IAuthService authService
            )
        {
            this.securityService = securityService;
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
                await logger.LogAsync("FormController", "Успешное добавление лота", LogState.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController",error,LogState.Error);
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
                await logger.LogAsync("FormController","Успешное добавление файла", LogState.Success);
            }
            catch(Exception ex)
            {
                await logger.LogAsync("FormController",$"Ошибка при добавлении файла: {ex.Message}", LogState.Error);
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
                await logger.LogAsync("FormController", "Успешное добавление предмета", LogState.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController",error,LogState.Error);
            return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        [Route("/Main/Register")]
        public async Task<IActionResult> RegisterUser(UserAuthViewModel userFormData)
        {
            await authService.Register(userFormData.UserName, userFormData.Password);
            return await LoginUser(userFormData);
        }
      
        [HttpPost]
        [Route("/Main/Login")]
        public async Task<IActionResult> LoginUser(UserAuthViewModel userFormData)
        {
            try
            {
                var jwtToken = await authService.Login(userFormData.UserName, userFormData.Password);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                HttpContext.Response.Cookies.Append("myToken", tokenString);    
                return RedirectToAction("Index", "Main");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}