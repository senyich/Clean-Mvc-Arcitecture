namespace Auction.Domain.Entities
{
    public class UserEntity
    {
        public int Id {get;set;}
        public string UserName {get;set;}
        public string PasswordHash {get;set;}
        public ICollection<ItemEntity> Games {get;set;}
    }
}


