using System.ComponentModel.DataAnnotations;

namespace Blog_Web.Models;

public class Blog
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık zorunludur.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "İçerik zorunludur.")]
    public string Content { get; set; }

    public DateTime PublishedDate { get; set; }

    //seçilen sürükleme
    [Required(ErrorMessage = "Kategori seçmelisiniz.")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    
    public string? ImagePath { get; set; }
}