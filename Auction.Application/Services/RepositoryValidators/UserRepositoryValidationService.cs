using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;
using Auction.Domain.Repositories;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Application.Services
{
    public class UserRepositoryValidationService : IUserValidationService
    {
        private IConverter<UserEntity, UserModel> entityConverter;
        private IConverter<UserModel, UserEntity> modelConverter;
        private IDbRepository<UserEntity> dbRepository;
        private ILoggerRepository logger;
        public UserRepositoryValidationService(
            ILoggerRepository logger, 
            IDbRepository<UserEntity> dbRepository,
            IConverter<UserEntity,UserModel> entityConverter,
            IConverter<UserModel,UserEntity> modelConverter)
        {
            this.dbRepository = dbRepository;
            this.logger = logger;
            this.entityConverter = entityConverter;
            this.modelConverter = modelConverter;
        }
        public async Task<int> AddUserAsync(UserModel user)
        {
            try
            {
                var userEntity = await modelConverter.Convert(user);
                var id = await dbRepository.Add(userEntity);
                await logger.LogAsync("UserValidator", $"Данные о пользователе  №{userEntity.Id} были занесены успешно", LogState.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"add data - {ex.Message}", LogState.Error);
                return -1;
            }
        }
        public async Task RemoveUserAsync(int id)
        {
            try
            {
                await dbRepository.Delete(id);
                await logger.LogAsync("UserValidator", $"Пользователь №{id} был удален успешно", LogState.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"rm data - {ex.Message}", LogState.Error);
            }
        }
        public async Task UpdateUserAsync(int id, UserModel user)
        {
            try
            {
                var userEntity = await modelConverter.Convert(user);
                await dbRepository.Update(id, userEntity);
                await logger.LogAsync("UserValidator", $"Пользователь №{id} был обновлен успешно", LogState.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"upd data - {ex.Message}", LogState.Error);
            }
        }
        public async Task<UserModel> GetUserAsync(int id)
        {
            try
            {
                var user = await dbRepository.Get(id);
                await logger.LogAsync("UserValidator", $"Пользователь №{id} был получен успешно", LogState.Success);
                return await entityConverter.Convert(user);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"get single data - {ex.Message}", LogState.Error);
                return null;
            }
        }
        public async Task<List<UserModel>> GetUsersAsync()
        {
            try
            {
                var users = await dbRepository.GetAll();
                await logger.LogAsync("UserValidator", $"Пользователи были получены успешно", LogState.Success);
                return users.Select(async l => await entityConverter.Convert(l))
                    .Select(t=>t.Result)
                    .ToList();//с ToListAsync() не сработало
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"get all data - {ex.Message}", LogState.Error);
                return null;
            }
        }
    }
}


