using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Application.Services
{
    public class AuctionRepositoryValidationService : IAuctionValidationService
    {
        private IConverter<OrderEntity, OrderModel> orderEntityToModelConverter;
        private IConverter<OrderModel, OrderEntity> orderModelToEntityConverter;
        private IDbRepository<OrderEntity> orderDbRepository;
        private ILoggerService logger;
        public AuctionRepositoryValidationService(
            ILoggerService logger,
            IDbRepository<OrderEntity> orderDbRepository, 
            IConverter<OrderEntity, OrderModel> orderEntityToModelConverter,
            IConverter<OrderModel, OrderEntity> orderModelToEntityConverter)
        {
            this.logger = logger;
            this.orderDbRepository = orderDbRepository;
            this.orderEntityToModelConverter = orderEntityToModelConverter;
            this.orderModelToEntityConverter = orderModelToEntityConverter;
        }
        public async Task<int> CreateOrderAsync(OrderModel orderModel)
        {
            try
            {
                var orderEntity = await orderModelToEntityConverter.ConvertAsync(orderModel);
                int id = await orderDbRepository.Add(orderEntity);
                await logger.LogAsync("OrderValidator", $"данные об ордере №{orderEntity.Id} были занесены успешно", LogType.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("OrderValidator", $"добавление данных - {ex.Message}", LogType.Error);
                return -1;
            }
        }
        public async Task RemoveOrderAsync(int id)
        {
            try
            {
                await orderDbRepository.Delete(id);
                await logger.LogAsync("OrderValidator", $"ордер №{id} был удален успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("OrderValidator", $"удаление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task UpdateOrderAsync(int id, OrderModel orderModel)
        {
            try
            {
                var orderEntity = await orderModelToEntityConverter.ConvertAsync(orderModel);
                await orderDbRepository.Update(id, orderEntity);
                await logger.LogAsync("OrderValidator", $"ордер №{id} был обновлен успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("OrderValidator", $"обновление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task<OrderModel?> GetSingleOrderAsync(int id)
        {
            try
            {
                var auction = await orderDbRepository.Get(id);
                await logger.LogAsync("OrderValidator", $"ордер №{id} был получен успешно", LogType.Success);
                return await orderEntityToModelConverter.ConvertAsync(auction);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("OrderValidator", $"получение данных - {ex.Message}", LogType.Error);
                return null;
            }
        }
        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            try
            {
                var lots = await orderDbRepository.GetAll();
                await logger.LogAsync("OrderValidator", $"все ордеры получены успешно", LogType.Success);
                return lots.Select(async lot => await orderEntityToModelConverter.ConvertAsync(lot))
                    .Select(t => t.Result)
                    .ToList();
            }
            catch (Exception ex)
            {
                await logger.LogAsync("OrderValidator", $"получение всех данных - {ex.Message}", LogType.Error);
                return new List<OrderModel>();
            }
        }
    }

}

