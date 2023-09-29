using System.ComponentModel.DataAnnotations;
namespace Forum2.Models;

public class Account
{
    public int AccountId { get; set; }
    [Required]
    public string AccountName { get; set; } = string.Empty;
    [Required]
    public string AccountPassword { get; set; } = string.Empty;
    public string? AccountAvatar { get; set; }
    public int? AccountRoleType { get; set; }
    //Navigation Property
    // public virtual List<AccountRoles>? AccountRolesList { get; set; }
}