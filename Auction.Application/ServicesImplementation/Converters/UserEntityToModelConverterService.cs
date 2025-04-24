using OrderWebsite.Application.Abstractions.IServices;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class UserEntityToModelConverterService : IConverterService<UserEntity, UserModel>
    {
        public async Task<UserModel> ConvertAsync(UserEntity userEntity)
        {
            (UserModel model, string error) user = UserModel.Create(
                userEntity.Id, 
                userEntity.UserName,
                userEntity.PasswordHash,
                userEntity.Balance);
            if(user.model!=null)
                return user.model;
            else
                throw new ArgumentNullException(user.error);
        }
    }
}