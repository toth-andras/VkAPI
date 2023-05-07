namespace VkAPI.DAL;

/// <summary>
/// Возвращается при попытке добавить второго(или последующего) администратора.
/// </summary>
public class MultipleAdminError
{
    public static MultipleAdminError Empty { get; } = new MultipleAdminError();
}