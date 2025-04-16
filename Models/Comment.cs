using System.ComponentModel.DataAnnotations;

namespace Blog_Web.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Kullanıcı
    public string UserId { get; set; }
    public AppUser User { get; set; }

    // Blog
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}