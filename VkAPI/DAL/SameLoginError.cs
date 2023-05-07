namespace VkAPI.DAL;

/// <summary>
/// Возвращается при попытке в течение сохранения одного
/// пользователя сохранить другого с таким же именем.
/// </summary>
public class SameLoginError
{
    public static SameLoginError Empty { get; } = new SameLoginError();
}