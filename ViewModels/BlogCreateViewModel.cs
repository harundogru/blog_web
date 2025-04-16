using System.ComponentModel.DataAnnotations;

namespace Blog_Web.ViewModels;

public class BlogCreateViewModel
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [Display(Name = "Başlık")]
    public string Title { get; set; }

    [Required(ErrorMessage = "İçerik zorunludur.")]
    [Display(Name = "İçerik")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Kategori seçilmelidir.")]
    [Display(Name = "Kategori")]
    public int CategoryId { get; set; }
    
    public IFormFile? Image { get; set; }
}