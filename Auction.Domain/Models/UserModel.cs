using System.Text;

namespace OrderWebsite.Domain.Models
{
    public class UserModel
    {
        private UserModel(int id, string userName, string passwordHash, decimal balance)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Balance = balance;
        }
        public int Id { get; }
        public string UserName { get; }
        public string PasswordHash { get; }
        public decimal Balance { get; }
        public static (UserModel model, string error) Create(int id, string userName, string passwordHash, decimal balance)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(userName))
                errorBuilder.Append("Имя или пароль пустые!");
            if (errorBuilder.Length != 0)
                return (null, errorBuilder.ToString())!;
            UserModel user = new UserModel(id, userName, passwordHash, balance);
            return (user, string.Empty);
        }

    }
}

