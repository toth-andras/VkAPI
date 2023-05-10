namespace VkAPI.Models.RequestModels;

/// <summary>
/// Модель запроса на создание пользователя. 
/// </summary>
/// <param name="Login">Логин пользователя.</param>
/// <param name="Password">Пароль пользователя.</param>
/// <param name="UserGroupCode">Тип пользователя: обычный или администратор.</param>
public record AddUserRequestModel(
    string Login,
    string Password,
    UserGroupCode UserGroupCode);