namespace VkAPI.Options;

/// <summary>
/// Используется для сохранения(чтения) строки подключения в (из) файлов настроек.
/// </summary>
public class PostgresOptions
{
    public string ConnectionString { get; set; }
}