using System.ComponentModel.DataAnnotations;

namespace Forum2.Models;

public class AccountRoles
{
    [Key]
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string RoleDescription { get; set; } = string.Empty;
    //Navigation Property
    // public virtual Account Account { get; set; } = default!;
}