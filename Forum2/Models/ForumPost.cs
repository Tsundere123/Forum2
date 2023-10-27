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

    public DateTime ForumPostLastEditedTime { get; set; } = DateTime.MinValue;

    [Required] 
    public bool ForumPostIsSoftDeleted { get; set; } = false;
    
    public string ForumPostLastEditedBy { get; set; } = string.Empty;
    // Navigation Property
    public virtual ForumThread? ForumThread { get; set; }
}