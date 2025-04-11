using Auction.Domain.Models;

namespace Auction.Application.Abstractions
{
    public interface IAuctionValidationService
    {
        Task<int> AddAuctionLotAsync(AuctionModel auction);
        Task<List<AuctionModel>> GetAllLotsAsync();
        Task<AuctionModel> GetAuctionLotAsync(int id);
        Task RemoveAuctionLotAsync(int id);
        Task UpdateAuctionLotAsync(int id, AuctionModel auction);
    }
}