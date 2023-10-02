using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumListViewModel
{
    public IEnumerable<Account> Accounts;
    public string? CurrentViewName;

    public ForumListViewModel(IEnumerable<Account> accounts, string? currentViewName)
    {
        Accounts = accounts;
        CurrentViewName = currentViewName;
    }
}