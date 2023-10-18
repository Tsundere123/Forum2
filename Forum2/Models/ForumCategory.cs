using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum2.Models;

public class ForumCategory
{
    [Key]
    public int ForumCategoryId { get; set; }
    [Required]
    public string ForumCategoryName { get; set; } = string.Empty;
    [Required]
    public string ForumCategoryDescription { get; set; } = string.Empty;
    // public virtual ICollection<ForumThread>? ForumThreads{ get; set; }
    public virtual List<ForumThread>? ForumThreads { get; set; }
}