namespace VkAPI.Models.RequestModels;

/// <summary>
/// Модель запроса на создание пользователя. 
/// </summary>
public record AddUserRequestModel(
    string Login,
    string Password,
    UserGroupCode UserGroupCode);