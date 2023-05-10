namespace VkAPI.Models.RequestModels;

/// <summary>
/// Модель запроса на получение пользователя. 
/// </summary>
/// <param name="UserId">Id пользователя.</param>
public record GetUserRequestModel(int UserId);