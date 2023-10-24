using System.ComponentModel.DataAnnotations;

namespace Forum2.Models;

public class WallPost
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string AuthorId { get; set; } = string.Empty;
    
    [Required]
    public string ProfileId { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public virtual List<WallPostReply> Replies { get; set; } = new();
}