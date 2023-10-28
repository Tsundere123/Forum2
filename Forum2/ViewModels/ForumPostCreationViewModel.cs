using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumPostCreationViewModel
{
    public IEnumerable<ApplicationUser> Accounts { get; set; }
    public ForumThread ForumThread { get; set; }
    public ForumPost ForumPost { get; set; }
}