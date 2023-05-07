namespace VkAPI.DAL;

using OneOf;
using OneOf.Types;
using Models;
using Models.RequestModels;

/// <summary>
/// Инкапсулирует логику обращения к данным с бизнес-логикой.
/// </summary>
/// <remarks>
/// В программе присутствуют как реализующие IUsersDataAccessLayer классы, UsersDbContext -
/// в последнем реализована только CRUD-логика, все проверки и бизнес-логика инкапсулированы
/// в реализующих IUsersDataAccessLayer классах.
/// </remarks>
public interface IUsersDataAccessLayer
{
    public Task<OneOf<User, NotFound>> GetUser(GetUserRequestModel requestModel);
    public Task<List<User>> GetUsers();
    public Task<OneOf<Success, MultipleAdminError, SameLoginError>> AddUser(AddUserRequestModel requestModel);
    public Task<OneOf<Success, NotFound>> RemoveUser(RemoveUserRequestModel requestModel);
}