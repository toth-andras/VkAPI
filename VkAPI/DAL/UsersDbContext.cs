using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using VkAPI.Models;

namespace VkAPI.DAL;

/// <summary>
/// Инкапсулирует CRUD-логику работы с данными.
/// </summary>
/// <remarks>
/// В программе присутствуют как реализующие IUsersDataAccessLayer классы, UsersDbContext -
/// в последнем реализована только CRUD-логика, все проверки и бизнес-логика инкапсулированы
/// в реализующих IUsersDataAccessLayer классах.
/// </remarks>
public class UsersDbContext : DbContext
{
    private readonly string _connectionString;

    private DbSet<User> Users { get; set; }

    public UsersDbContext(string connectionString)
    {
        _connectionString = connectionString;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    public bool HasAdmin()
    {
        // Так как удаление происходит выставлением статуса Blocked, ищем тех администраторов,
        // у которых статус Active.
        return Users
            .Any(x => x.UserGroup.Code == UserGroupCode.Admin && x.UserState.Code == UserStateCode.Active);
    }

    public async Task<OneOf<User, NotFound>> GetUser(int userId)
    {
        var user = await Users.FindAsync(userId);
        return user is null
            ? new NotFound()
            : user!;
    }

    public async Task<List<User>> GetUsers()
    {
        return await Users.ToListAsync();
    }

    public async Task AddUser(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
    }

    public async Task<OneOf<Success, NotFound>> RemoveUser(int userId)
    {
        var user = await Users.FindAsync(userId);
        if (user is null)
        {
            return new NotFound();
        }

        user.UserState.Code = UserStateCode.Blocked;
        await SaveChangesAsync();
        return new Success();
    }
}