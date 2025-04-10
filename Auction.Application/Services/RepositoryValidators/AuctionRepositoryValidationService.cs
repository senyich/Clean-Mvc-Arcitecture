using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class AuctionRepositoryValidationService : IAuctionValidationService
    {
        private IConverter<AuctionEntity, AuctionModel> entityConverter;
        private IConverter<AuctionModel, AuctionEntity> modelConverter;
        private IDbRepository<AuctionEntity> dbRepository;
        private ILoggerService logger;
        public AuctionRepositoryValidationService(
            ILoggerService logger,
            IDbRepository<AuctionEntity> dbRepository, 
            IConverter<AuctionEntity, AuctionModel> entityConverter,
            IConverter<AuctionModel, AuctionEntity> modelConverter)
        {
            this.dbRepository = dbRepository;
            this.logger = logger;
            this.entityConverter = entityConverter;
            this.modelConverter = modelConverter;
        }
        public async Task<int> AddAuctionLotAsync(AuctionModel auction)
        {
            try
            {
                var auctionEntity = await modelConverter.Convert(auction);
                int id = await dbRepository.Add(auctionEntity);
                await logger.LogAsync("AuctionValidator", $"Данные о лоте №{auctionEntity.Id} были занесены успешно", LogState.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"add data - {ex.Message}", LogState.Error);
                return -1;
            }
        }
        public async Task RemoveAuctionLotAsync(int id)
        {
            try
            {
                await dbRepository.Delete(id);
                await logger.LogAsync("AuctionValidator", $"Лот №{id} был удален успешно", LogState.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"rm data - {ex.Message}", LogState.Error);
            }
        }
        public async Task UpdateAuctionLotAsync(int id, AuctionModel auction)
        {
            try
            {
                var auctionEntity = await modelConverter.Convert(auction);
                await dbRepository.Update(id, auctionEntity);
                await logger.LogAsync("AuctionValidator", $"Лот №{id} был обновлен успешно", LogState.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"upd data - {ex.Message}", LogState.Error);
            }
        }
        public async Task<AuctionModel> GetAuctionLotAsync(int id)
        {
            try
            {
                var auction = await dbRepository.Get(id);
                await logger.LogAsync("AuctionValidator", $"Лот №{id} был получен успешно", LogState.Success);
                return await entityConverter.Convert(auction);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"get single data - {ex.Message}", LogState.Error);
                return null;
            }
        }
        public async Task<List<AuctionModel>> GetAllLotsAsync()
        {
            try
            {
                var lots = await dbRepository.GetAll();
                await logger.LogAsync("AuctionValidator", $"Все лоты получены успешно", LogState.Success);
                var list = lots.Select(async l => await entityConverter.Convert(l))
                    .Select(t=>t.Result)
                    .ToList();
                return list;//с ToListAsync() не сработало
            }
            catch (Exception ex)
            {
                await logger.LogAsync("AuctionValidator", $"get all data - {ex.Message}", LogState.Error);
                return null;
            }
        }
    }

}

