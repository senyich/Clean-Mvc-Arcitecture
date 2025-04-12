using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Application.Services
{
    public class AuctionRepositoryValidationService : IAuctionValidationService
    {
        private IConverter<AuctionEntity, AuctionModel> auctionEntityToModelConverter;
        private IConverter<AuctionModel, AuctionEntity> auctionModelToEntityConverter;
        private IDbRepository<AuctionEntity> auctionDbRepository;
        private ILoggerService logger;
        public AuctionRepositoryValidationService(
            ILoggerService logger,
            IDbRepository<AuctionEntity> auctionDbRepository, 
            IConverter<AuctionEntity, AuctionModel> auctionEntityToModelConverter,
            IConverter<AuctionModel, AuctionEntity> auctionModelToEntityConverter)
        {
            this.logger = logger;
            this.auctionDbRepository = auctionDbRepository;
            this.auctionEntityToModelConverter = auctionEntityToModelConverter;
            this.auctionModelToEntityConverter = auctionModelToEntityConverter;
        }
        public async Task<int> AddAuctionLotAsync(AuctionModel auction)
        {
            try
            {
                var auctionEntity = await auctionModelToEntityConverter.ConvertAsync(auction);
                if (auctionEntity == null)
                    throw new ArgumentNullException();
                int id = await auctionDbRepository.Add(auctionEntity);
                await logger.LogAsync("AuctionValidator", $"данные о лоте №{auctionEntity.Id} были занесены успешно", LogType.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"добавление данных - {ex.Message}", LogType.Error);
                return -1;
            }
        }
        public async Task RemoveAuctionLotAsync(int id)
        {
            try
            {
                await auctionDbRepository.Delete(id);
                await logger.LogAsync("AuctionValidator", $"лот №{id} был удален успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"удаление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task UpdateAuctionLotAsync(int id, AuctionModel auction)
        {
            try
            {
                var auctionEntity = await auctionModelToEntityConverter.ConvertAsync(auction);
                await auctionDbRepository.Update(id, auctionEntity);
                await logger.LogAsync("AuctionValidator", $"лот №{id} был обновлен успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"обновление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task<AuctionModel?> GetSingleAuctionLotAsync(int id)
        {
            try
            {
                var auction = await auctionDbRepository.Get(id);
                if (auction == null)
                    throw new ArgumentNullException();
                await logger.LogAsync("AuctionValidator", $"лот №{id} был получен успешно", LogType.Success);
                return await auctionEntityToModelConverter.ConvertAsync(auction);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"получение данных - {ex.Message}", LogType.Error);
                return null;
            }
        }
        public async Task<List<AuctionModel>?> GetAllLotsAsync()
        {
            try
            {
                var lots = await auctionDbRepository.GetAll();
                await logger.LogAsync("AuctionValidator", $"все лоты получены успешно", LogType.Success);
                return lots.Select(async lot => await auctionEntityToModelConverter.ConvertAsync(lot))
                    .Select(t => t.Result)
                    .ToList();
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"получение всех данных - {ex.Message}", LogType.Error);
                return null;
            }
        }
    }

}

