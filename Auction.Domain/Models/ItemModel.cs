using System.Text;

namespace Auction.Domain.Models
{
    public class ItemModel
    {
        private ItemModel(int id,string name, string description, string imgPath, int auctionId, int ownerId)
        {
            Id = id;
            Name = name;
            Description = description;
            ImgPath = imgPath;
            AuctionId = auctionId;
            OwnerId = ownerId;
        }
        public int Id {get;}
        public int AuctionId {get;}
        public int OwnerId { get; }
        public string Name {get;}
        public string Description {get;}
        public string ImgPath {get;}
        public static (ItemModel model, string error) Create(int id,string name, string description, string imgPath, int auctionId, int ownerId)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if(string.IsNullOrEmpty(name))
            {
                errorBuilder.Append("Имя пустое!");
                return (null, errorBuilder.ToString())!;
            }
            ItemModel gameModel = new ItemModel(id,name, description, imgPath, auctionId, ownerId);
            return (gameModel, string.Empty);
        }
    }
}

