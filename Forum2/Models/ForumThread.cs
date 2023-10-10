﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum2.Models;

public class ForumThread
{
    [Key]
    public int ForumThreadId { get; set; }
    [Required]
    public string ForumThreadTitle { get; set; } = string.Empty;
    [Required]
    [ForeignKey("ForumCategoryId")]
    public int ForumCategoryId { get; set; }
    // [Required]
    // [ForeignKey("AccountId")]
    // public int AccountId { get; set; }
    
    public int ForumThreadCreatorId { get; set; }
    
    // //Navigation Property
    // public virtual Account? Account { get; set; }
    
    // //Navigation Property
    public virtual ForumCategory? ForumCategory { get; set; }
}