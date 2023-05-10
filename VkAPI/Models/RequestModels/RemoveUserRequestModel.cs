namespace VkAPI.Models.RequestModels;

/// <summary>
/// Модель запроса на удаление пользователя. 
/// </summary>
/// <param name="UserId">Id пользователя.</param>
public record RemoveUserRequestModel(int UserId);