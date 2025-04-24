using System.Text;

namespace OrderWebsite.Domain.Models
{
    public class ItemModel
    {
        private ItemModel(int id,string name, string description, string imgPath, int orderId, int ownerId)
        {
            Id = id;
            Name = name;
            Description = description;
            ImgPath = imgPath;
            OrderId = orderId;
            OwnerId = ownerId;
        }
        public int Id { get; }
        public int OrderId { get; }
        public int OwnerId { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImgPath { get; }
        public static (ItemModel? model, string error) Create(int id, string name, string description, string imgPath, int orderId, int ownerId)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if(string.IsNullOrEmpty(name))
                errorBuilder.Append("Имя пустое!");
            if (errorBuilder.Length != 0)
                return (null, errorBuilder.ToString());

            ItemModel gameModel = new ItemModel(id, name, description, imgPath, orderId, ownerId);
            return (gameModel, string.Empty);
        }
    }
}

