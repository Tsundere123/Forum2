using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum2.Models;

public class ForumPost
{
    [Key]
    public int ForumPostId { get; set; }

    [Required]
    [ForeignKey("ForumThreadId")]
    public int ForumThreadId { get; set; } = default!;
    
    [Required]
    [ForeignKey("Id")]
    public string ForumPostCreatorId { get; set; } = string.Empty;
    
    [Required]
    public string ForumPostContent { get; set; } = string.Empty;
    
    [Required]
    public DateTime ForumPostCreationTimeUnix { get; set; } = DateTime.MinValue;
    // Navigation Property
    public virtual ForumThread? ForumThread { get; set; }
}