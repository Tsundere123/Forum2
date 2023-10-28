using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum2.Models;

public class ForumCategory
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    // public virtual ICollection<ForumThread>? ForumThreads{ get; set; }
    public virtual List<ForumThread>? Threads { get; set; }
}