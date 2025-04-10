﻿using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class AuctionModelConverterService : IConverter<AuctionModel, AuctionEntity>
    {
        public async Task<AuctionEntity> Convert(AuctionModel model)
        {
            var auction = new AuctionEntity();
            auction.BuyPrice = model.BuyPrice;
            auction.CurrentPrice = model.CurrentPrice;
            auction.ItemId = model.ItemId;
            auction.MinPriceUpdateRate = model.MinPriceUpdateRate;
            return auction;
        }
    }
}