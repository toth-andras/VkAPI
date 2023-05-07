namespace VkAPI.Models;

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum UserGroupCode
{
    Admin = 0,
    User = 1
}

[Owned]
[Table("UserGroups")]
public class UserGroup
{
    [Key] [Column("id")] public int Id { get; set; }

    [JsonIgnore] [Column("code")] public UserGroupCode Code { get; set; }

    [NotMapped]
    [JsonPropertyName("code")]
    public string CodeString
    {
        get => Code.ToString();
        set => Code = Enum.Parse<UserGroupCode>(value);
    }

    [Column("description")] public string Description { get; set; }
}