using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Application.Abstractions.IValidators;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Enums;
using OrderWebsite.Domain.Models;
using OrderWebsite.Domain.Repositories;

namespace OrderWebsite.Application.Services
{
    public class OrderRepositoryValidationService : IOrderValidationService
    {
        private IConverterService<OrderEntity, OrderModel> orderEntityToModelConverter;
        private IConverterService<OrderModel, OrderEntity> orderModelToEntityConverter;
        private IOrderRepository orderRepository;
        private ILoggerService logger;
        public OrderRepositoryValidationService(
            ILoggerService logger,
            IOrderRepository orderRepository, 
            IConverterService<OrderEntity, OrderModel> orderEntityToModelConverter,
            IConverterService<OrderModel, OrderEntity> orderModelToEntityConverter)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
            this.orderEntityToModelConverter = orderEntityToModelConverter;
            this.orderModelToEntityConverter = orderModelToEntityConverter;
        }
        public async Task<int> CreateOrderAsync(OrderModel orderModel)
        {
            try
            {
                var orderEntity = await orderModelToEntityConverter.ConvertAsync(orderModel);
                int id = await orderRepository.Add(orderEntity);
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
                await orderRepository.Delete(id);
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
                await orderRepository.Update(id, orderEntity);
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
                var auction = await orderRepository.Get(id);
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
                var lots = await orderRepository.GetAll();
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

