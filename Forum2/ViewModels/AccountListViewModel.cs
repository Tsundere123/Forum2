using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class AccountListViewModel
    {
        public IEnumerable<ApplicationUser> Accounts;
        public string? CurrentViewName;

        public AccountListViewModel(IEnumerable<ApplicationUser> accounts, string? currentViewName)
        {
            Accounts = accounts;
            CurrentViewName = currentViewName;
        }
    }
}