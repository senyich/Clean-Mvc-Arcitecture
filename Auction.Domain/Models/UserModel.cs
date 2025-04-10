using System.Text;

namespace Auction.Domain.Models
{
    public class UserModel
    {
        private UserModel(int id, string userName, string passwordHash)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
        }
        public int Id {get;}
        public string UserName {get;}
        public string PasswordHash {get;}
        public static (UserModel model, string error) Create(int id, string userName, string passwordHash)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if(string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(userName))
                errorBuilder.Append("Имя или пароль пустые!");
            if(!userName[0].Equals("@"))
                errorBuilder.Append("Имя или пароль пустые!"); 
            if(errorBuilder.Length != 0)
                return (null, errorBuilder.ToString())!;
            UserModel user = new UserModel(id,userName,passwordHash);
            return (user, string.Empty);
        }

    }
}

