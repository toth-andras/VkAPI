namespace VkAPI.Models;

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum UserStateCode
{
    Active = 0,
    Blocked = 1
}

[Owned]
[Table("UserStates")]
public class UserState
{
    [Key] [Column("id")] public int Id { get; set; }

    [JsonIgnore] [Column("code")] public UserStateCode Code { get; set; }

    [NotMapped]
    [JsonPropertyName("code")]
    public string CodeString
    {
        get => Code.ToString();
        set => Code = Enum.Parse<UserStateCode>(value);
    }

    [Column("description")] public string Description { get; set; }
}