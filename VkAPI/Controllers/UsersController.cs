namespace VkAPI.Controllers;

using DAL;
using Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер для работы с данными о пользователях.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersDataAccessLayer _dataAccessLayer;

    public UsersController(ILogger<UsersController> logger, IUsersDataAccessLayer accessLayer)
    {
        _logger = logger;
        _dataAccessLayer = accessLayer;
    }

    /// <summary>
    /// Получить всех пользователей из базы данных.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetUsers")]
    [Produces("application/json")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return StatusCode(200, await _dataAccessLayer.GetUsers());
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Получить из базы данных конкретного пользователя.
    /// </summary>
    /// <param name="requestModel">Модель с данными запроса информации о конкретном пользователе.</param>
    /// <returns>Ответ, содержащий данные о пользователе или ошибке.</returns>
    [HttpPost("GetUser")]
    [Produces("application/json")]
    public async Task<IActionResult> GetUser([FromBody] GetUserRequestModel requestModel)
    {
        try
        {
            return (await _dataAccessLayer.GetUser(requestModel))
                .Match(
                    user => StatusCode(200, user),
                    notFound => StatusCode(400, $"No user with such id: {requestModel.UserId}")
                );
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Добавить пользователя в базу данных.
    /// </summary>
    /// <param name="requestModel">Модель с данными запроса на добавление пользователя.</param>
    /// <returns>Ответ, содержащий сообщение об успехе или данные об ошибке.</returns>
    [HttpPost("AddUser")]
    [Produces("application/json")]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequestModel requestModel)
    {
        try
        {
            return (await _dataAccessLayer.AddUser(requestModel))
                .Match(
                    success => StatusCode(200, "User has been added to database"),
                    adminError => StatusCode(400, "Cannot have multiple admins"),
                    loginError => StatusCode(400, "Cannot add a user with the same login."));
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Удалить пользователя из базы данных.
    /// </summary>
    /// <param name="requestModel">Модель с данными запроса на удаление пользователя.</param>
    /// <returns>Ответ, содержащий сообщение об успехе или данные об ошибке.</returns>
    [HttpPost("RemoveUser")]
    [Produces("application/json")]
    public async Task<IActionResult> RemoveUser([FromBody] RemoveUserRequestModel requestModel)
    {
        try
        {
            return (await _dataAccessLayer.RemoveUser(requestModel))
                .Match(
                    success => StatusCode(200, $"User with id {requestModel.UserId} has been removed"),
                    notfound => StatusCode(400, $"No user with such id: {requestModel.UserId}")
                );
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return StatusCode(500, e.Message);
        }
    }
}