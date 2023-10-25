using System.Collections;
using Forum2.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum2.ViewModels;

public class RoleViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public ApplicationRole CurrentRole;

    public RoleViewModel(IEnumerable<ApplicationUser> applicationUsers, ApplicationRole currentRole)
    {
        Accounts = applicationUsers;
        CurrentRole = currentRole;
    }
}