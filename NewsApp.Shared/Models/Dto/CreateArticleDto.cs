using System.ComponentModel.DataAnnotations;

namespace NewsApp.Shared.Models.Dto;

public class CreateArticleDto
{
    
    [Required(ErrorMessage = "Title is required")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
    public string Title { get; set; }
    
    public Guid? CategoryId { get; set; }

    public string Author { get; set; }

    public string AuthorId { get; set; }

    
    [Required(ErrorMessage = "Content is required")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
    public string Content { get; set; }
    public bool IsPremium { get; set; }
}