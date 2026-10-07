using Core.Enums;

namespace Model.Entities;

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Operator;
    public bool IsActive { get; set; } = true;
}
