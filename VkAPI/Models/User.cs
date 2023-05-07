namespace VkAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public record User(string Login, string Password, DateTime CreatedDate)
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("login")] public string Login { get; set; } = Login;

    [Column("password")] public string Password { get; set; } = Password;

    [Column("created_date")] public DateTime CreatedDate { get; set; } = CreatedDate;

    [Column("user_group")] public UserGroup UserGroup { get; set; }

    [Column("user_state")] public UserState UserState { get; set; }
}