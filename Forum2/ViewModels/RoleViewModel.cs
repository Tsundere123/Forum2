using System.Collections;
using Forum2.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum2.ViewModels;

public class RoleViewModel
{
    public IEnumerable<ApplicationUser> Accounts { get; set; }
    public ApplicationRole CurrentRole { get; set; }
}