namespace VkAPI.DAL;

using OneOf;
using Models;
using Options;
using OneOf.Types;
using Models.RequestModels;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

/// <summary>
/// Конкретная реализация доступа к данным с поддержкой бизнес-логики.
/// </summary>
/// <remarks>
/// В программе присутствуют как реализующие IUsersDataAccessLayer классы, UsersDbContext -
/// в последнем реализована только CRUD-логика, все проверки и бизнес-логика инкапсулированы
/// в реализующих IUsersDataAccessLayer классах.
/// </remarks>
public class UsersDataAccessLayer : IUsersDataAccessLayer
{
    private readonly string _connectionString;

    // Здесь будут храниться логины пользователей в то время, пока они добавляются базу данных.
    // Используем Dictionary для проверки за O(1) и для возможности удаления объектов.
    private readonly ConcurrentDictionary<string, bool> _loginsInProcess;

    public UsersDataAccessLayer(IOptions<PostgresOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
        _loginsInProcess = new ConcurrentDictionary<string, bool>();
    }

    public async Task<OneOf<User, NotFound>> GetUser(GetUserRequestModel requestModel)
    {
        await using var context = new UsersDbContext(_connectionString);
        return await context.GetUser(requestModel.UserId);
    }

    public async Task<List<User>> GetUsers()
    {
        await using var context = new UsersDbContext(_connectionString);
        return await context.GetUsers();
    }

    public async Task<OneOf<Success, MultipleAdminError, SameLoginError>> AddUser(AddUserRequestModel requestModel)
    {
        // Проверка на то, что пользователь с таким логином уже сохраняется.
        if (_loginsInProcess.ContainsKey(requestModel.Login))
        {
            return SameLoginError.Empty;
        }

        _loginsInProcess[requestModel.Login] = true;

        await using var context = new UsersDbContext(_connectionString);

        // Проверка на создания второго/последующего администратора.
        if (requestModel.UserGroupCode == UserGroupCode.Admin && context.HasAdmin())
        {
            return MultipleAdminError.Empty;
        }

        // Вариант улучшения: нормализация базы данных, не сохранять лишние UserGroup и UserState.
        // К сожалению, я не успел это сделать.
        var user = new User(
            requestModel.Login,
            requestModel.Password,
            DateTime.UtcNow)
        {
            UserGroup = new UserGroup()
            {
                Description = requestModel.UserGroupDescription,
                Code = requestModel.UserGroupCode
            },
            UserState = new UserState()
            {
                Description = requestModel.UserStateDescription,
                Code = UserStateCode.Active
            }
        };

        await context.AddUser(user);
        await Task.Delay(5000);
        _loginsInProcess.Remove(requestModel.Login, out _);

        return new Success();
    }

    public async Task<OneOf<Success, NotFound>> RemoveUser(RemoveUserRequestModel requestModel)
    {
        await using var context = new UsersDbContext(_connectionString);
        return await context.RemoveUser(requestModel.UserId);
    }
}