using System.Text;

namespace Auction.Domain.Models
{
    public class UserModel
    {
        private UserModel(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }
        public string UserName {get;}
        public string PasswordHash {get;}
        public static (UserModel model, string error) Create(string userName, string passwordHash)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if(string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(userName))
                errorBuilder.Append("Имя или пароль пустые!");
            if(!userName[0].Equals("@"))
                errorBuilder.Append("Имя или пароль пустые!"); 
            if(errorBuilder.Length != 0)
                return (null, errorBuilder.ToString())!;
            UserModel user = new UserModel(userName,passwordHash);
            return (user, string.Empty);
        }

    }
}

