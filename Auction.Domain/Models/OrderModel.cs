using System.Text;

namespace OrderWebsite.Domain.Models
{
    public class OrderModel
    {
        private OrderModel(int id, int itemId, int ownerId, decimal buyPrice)
        {
            Id = id;
            ItemId = itemId;
            OwnerId = ownerId;
            BuyPrice = buyPrice;
        }
        public int Id { get; }
        public int ItemId { get; }
        public int OwnerId { get; }
        public decimal BuyPrice {get;}
        public static (OrderModel model, string error) Create(int id,int itemId,int ownerId, decimal buyPrice)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if(buyPrice <= 0)
            {
                errorBuilder.Append("Недопустимые цены!");
                return (null, errorBuilder.ToString())!;
            }
            OrderModel auctionModel = new OrderModel(id, itemId, ownerId, buyPrice);
            return (auctionModel, string.Empty);
        }
    }

}

