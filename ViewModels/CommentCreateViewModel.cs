using System.ComponentModel.DataAnnotations;

namespace Blog_Web.ViewModels;

public class CommentCreateViewModel
{
    [Required]
    public string Content { get; set; }

    public int BlogId { get; set; }
}