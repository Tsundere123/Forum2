using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class AccountListViewModel
    {
        public IEnumerable<Account> Accounts;
        public string? CurrentViewName;

        public AccountListViewModel(IEnumerable<Account> accounts, string? currentViewName)
        {
            Accounts = accounts;
            CurrentViewName = currentViewName;
        }
    }
}