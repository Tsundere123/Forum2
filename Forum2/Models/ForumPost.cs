using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum2.Models;

public class ForumPost
{
    [Key]
    public int ForumPostId { get; set; }

    [Required]
    [ForeignKey("ForumThreadId")]
    public string ForumThreadId { get; set; } = string.Empty;
    
    [Required]
    [ForeignKey("Id")]
    public string ForumPostCreatorId { get; set; } = string.Empty;
    
    [Required]
    public string ForumPostContent { get; set; } = string.Empty;
    
    
}