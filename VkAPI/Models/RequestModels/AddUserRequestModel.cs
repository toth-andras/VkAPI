namespace VkAPI.Models.RequestModels;

/// <summary>
/// Модель запроса на создание пользователя. 
/// </summary>
/// <param name="Login">Логин пользователя.</param>
/// <param name="Password">Пароль пользователя.</param>
/// <param name="UserGroupCode">Тип пользователя: обычный или администратор.</param>
/// <param name="UserGroupDescription">Описание типа пользователя.</param>
/// <param name="UserStateDescription">Описание состояния пользователя.</param>
public record AddUserRequestModel(
    string Login,
    string Password,
    UserGroupCode UserGroupCode,
    string UserGroupDescription,
    string UserStateDescription);