using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Enums;
using OrderWebsite.Domain.Models;
using OrderWebsite.Domain.Repositories;

namespace OrderWebsite.Application.Services
{
    public class UserRepositoryValidationService : IUserValidationService
    {
        private IConverter<UserEntity, UserModel> userEntityToModelConverter;
        private IConverter<UserModel, UserEntity> userModelToEntityConverter;
        private IUserRepository userRepository;
        private ILoggerService logger;
        public UserRepositoryValidationService(
            ILoggerService logger,
            IUserRepository userRepository,
            IConverter<UserEntity,UserModel> userEntityToModelConverter,
            IConverter<UserModel,UserEntity> userModelToEntityConverter)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.userEntityToModelConverter = userEntityToModelConverter;
            this.userModelToEntityConverter = userModelToEntityConverter;
        }
        public async Task<int> CreateUserAsync(UserModel user)
        {
            try
            {
                var userEntity = await userModelToEntityConverter.ConvertAsync(user);
                if (userEntity == null)
                    throw new ArgumentNullException();
                var id = await userRepository.Add(userEntity);
                await logger.LogAsync("UserValidator", $"данные о пользователе  №{userEntity.Id} были занесены успешно", LogType.Success);
                return id;
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"добавление данных - {ex.Message}", LogType.Error);
                return -1;
            }
        }
        public async Task RemoveUserAsync(int id)
        {
            try
            {
                await userRepository.Delete(id);
                await logger.LogAsync("UserValidator", $"пользователь №{id} был удален успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"удаление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task UpdateUserAsync(int id, UserModel user)
        {
            try
            {
                var userEntity = await userModelToEntityConverter.ConvertAsync(user);
                await userRepository.Update(id, userEntity);
                await logger.LogAsync("UserValidator", $"пользователь №{id} был обновлен успешно", LogType.Success);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"обновление данных - {ex.Message}", LogType.Error);
            }
        }
        public async Task<UserModel?> GetSingleUserAsync(int id)
        {
            try
            {
                var user = await userRepository.Get(id);
                if (user == null)
                    throw new ArgumentNullException();
                await logger.LogAsync("UserValidator", $"пользователь №{id} был получен успешно", LogType.Success);
                return await userEntityToModelConverter.ConvertAsync(user);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"получение данных - {ex.Message}", LogType.Error);
                return null;
            }
        }
        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            try
            {
                var users = await userRepository.GetAll();
                await logger.LogAsync("UserValidator", $"пользователи были получены успешно", LogType.Success);
                return users.Select(async l => await userEntityToModelConverter.ConvertAsync(l))
                    .Select(t=>t.Result)
                    .ToList();
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"получение всех данных - {ex.Message}", LogType.Error);
                return new List<UserModel>();
            }
        }

        public async Task<UserModel?> GetSingleUserAsync(string username)
        {
            try
            {
                var user = await userRepository.GetSingleUserByUsername(username);
                if (user == null)
                    return null;
                await logger.LogAsync("UserValidator", $"пользователь №{user.Id} был получен успешно", LogType.Success);
                return await userEntityToModelConverter.ConvertAsync(user);
            }
            catch (Exception ex)
            {
                await logger.LogAsync("UserValidator", $"получение данных о пользователе по имени - {ex.Message}", LogType.Error);
                throw;
            }
        }
    }
}


