using Microsoft.AspNetCore.Mvc;
using OrderWebsite.Web.ViewModels;
using OrderWebsite.Domain.Models;
using OrderWebsite.Domain.Enums;
using OrderWebsite.Application.Abstractions;
using System.Security.Claims;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace OrderWebsite.Web.Controllers
{
    public class FormController : Controller
    {
        private IOrderValidationService auctionRepository;
        private IUserValidationService userValidator;
        private IAuthService authService;
        private IItemValidationService itemRepository;
        private ILoggerService logger;
        private IFileLogisticService fileLogisticService;
        private IWebHostEnvironment environment;
        public FormController(
            IOrderValidationService auctionRepository,
            ILoggerService logger,
            IFileLogisticService fileLogisticService,
            IWebHostEnvironment environment,
            IItemValidationService itemRepository,
            IUserValidationService userValidator,
            IAuthService authService
            )
        {
            this.itemRepository = itemRepository;
            this.auctionRepository = auctionRepository;
            this.logger = logger;
            this.fileLogisticService = fileLogisticService;
            this.environment = environment;
            this.authService = authService;
            this.userValidator = userValidator;
        }
        [HttpPost]
        [Route("/Api/CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var (auctionModel, error) = OrderModel.Create(0, model.ItemId, userId, model.BuyPrice);    
            if(string.IsNullOrEmpty(error))
            {          
                int id = await auctionRepository.CreateOrderAsync(auctionModel);

                var tmpItem = await itemRepository.GetSingleItemAsync(model.ItemId);

                var (updGame, gameError) = ItemModel.Create(tmpItem.Id, tmpItem.Name, tmpItem.Description, tmpItem.ImgPath, id, userId);
                
                await itemRepository.UpdateItemAsync(model.ItemId, updGame);
                await logger.LogAsync("FormController", "успешное добавление лота", LogType.Success);
                return RedirectToAction("Index", "Main");
            }
            await logger.LogAsync("FormController",error,LogType.Error);
            return RedirectToAction("Index", "Main");
        }
        [HttpPost]
        [Route("/Api/BuyOrder")]
        public async Task<IActionResult> BuyOrder(int orderId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return RedirectToAction("Authorization", "Main");

            var user = await userValidator.GetSingleUserAsync(int.Parse(userId));
            var item = (await itemRepository.GetAllItemsAsync()).FirstOrDefault(f => f.OrderId == orderId);
            var order = await auctionRepository.GetSingleOrderAsync(orderId);
            var owner = await userValidator.GetSingleUserAsync(order.OwnerId);

            (ItemModel model, string error) newItem = ItemModel.Create(item.Id, item.Name, item.Description, item.ImgPath, 0, user.Id);
            (UserModel model, string error) newUser = UserModel.Create(user.Id, user.UserName, user.PasswordHash, user.Balance - order.BuyPrice);
            (UserModel model, string error) newOwner = UserModel.Create(owner.Id, owner.UserName, owner.PasswordHash, owner.Balance + order.BuyPrice);

            await userValidator.UpdateUserAsync(user.Id, newUser.model);
            await userValidator.UpdateUserAsync(owner.Id, newOwner.model);
            await itemRepository.UpdateItemAsync(item.Id, newItem.model);
            await auctionRepository.RemoveOrderAsync(orderId);

            return RedirectToAction("Index", "Main");
        }
        [HttpPost]
        [Route("/Api/AddItem")]
        public async Task<IActionResult> AddItem(CreateItemViewModel item)
        {
            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            string filePath = await fileLogisticService.SaveFileAsync(item.ImageFile, environment.WebRootPath);

            var (itemModel, error) = ItemModel.Create(item.Id, item.Name, item.Description, filePath, 0, userId);

            if(string.IsNullOrEmpty(error))
            {
                await itemRepository.CreateItemAsync(itemModel);
                await logger.LogAsync("FormController", "успешное добавление предмета", LogType.Success);
                return RedirectToAction("Items", "Main");
            }
            await logger.LogAsync("FormController", $"ошибка добавления предмета - {error}", LogType.Error);
            return RedirectToAction("Items", "Main");
        }

        [HttpPost]
        [Route("/Api/Register")]
        public async Task<IActionResult> RegisterUser(UserAuthViewModel userFormData)
        {
            await authService.RegisterAsync(userFormData.UserName, userFormData.Password);
            return await LoginUser(userFormData);
        }
   
        [HttpPost]
        [Route("/Api/Login")]
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