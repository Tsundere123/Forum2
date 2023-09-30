using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum2.Models;

public class Account
{
    public int AccountId { get; set; }
    [Required]
    public string AccountName { get; set; } = string.Empty;
    [Required]
    public string AccountPassword { get; set; } = string.Empty;
    public string? AccountAvatar { get; set; }
    [ForeignKey("AccountRoles")]
    public int? RoleId { get; set; }
    //Navigation Property
    // public virtual ICollection<AccountRoles>? AccountRolesList { get; set; }
    public virtual AccountRoles? AccountRoles { get; set; }
}