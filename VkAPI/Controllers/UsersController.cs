namespace VkAPI.Controllers;

using DAL;
using Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("GetUser")]
    [Produces("application/json")]
    public async Task<IActionResult> GetUser([FromBody] GetUserRequestModel requestModel)
    {
        try
        {
            var result = await _dataAccessLayer.GetUser(requestModel);
            return result.IsT0
                ? StatusCode(200, result.AsT0)
                : StatusCode(400, $"No user with such id: {requestModel.UserId}");
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return StatusCode(500, e.Message);
        }
    }

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

    [HttpPost("RemoveUser")]
    [Produces("application/json")]
    public async Task<IActionResult> RemoveUser(RemoveUserRequestModel requestModel)
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