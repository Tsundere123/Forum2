using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public int ForumCategoryId { get; set; } = default!;
    // [Required]
    // [ForeignKey("AccountId")]
    // public int AccountId { get; set; }

    public string ForumThreadCreatorId { get; set; } = string.Empty;
    
    [Required]
    [DefaultValue(typeof(DateTime), "DateTime.UtcNow")]
    public DateTime ForumThreadCreationTimeUnix { get; set; } = DateTime.MinValue;
    
    public DateTime ForumThreadLastEditedTime { get; set; } = DateTime.MinValue;

    public string ForumThreadLastEditedBy { get; set; } = string.Empty;

    [Required] 
    public bool ForumThreadIsSoftDeleted { get; set; } = false;
    
    
    // //Navigation Property
    // public virtual Account? Account { get; set; }
    
    // //Navigation Property
    public virtual ForumCategory? ForumCategory { get; set; }
    
    //Navigation Property
    public virtual List<ForumPost>? ForumPosts { get; set; }
}