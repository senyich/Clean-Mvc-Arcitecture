using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class UserEntityConverterService : IConverter<UserEntity, UserModel>
    {
        public UserEntityConverterService()
        {

        }
        public async Task<UserModel> Convert(UserEntity obj)
        {
            var user = UserModel.Create(obj.Id, obj.UserName, obj.PasswordHash);
            if(user.model!=null)
            {
                return user.model;
            }
            else
            { 
                throw new ArgumentNullException(user.error);
            }
        }
    }
}