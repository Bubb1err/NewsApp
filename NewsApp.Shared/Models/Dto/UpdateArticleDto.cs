using System.ComponentModel.DataAnnotations;

namespace NewsApp.Shared.Models.Dto;

public class UpdateArticleDto
{
    public Guid ArticleId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Content is required")]
    [MinLength(3, ErrorMessage = "Content must be at least 3 characters long")]
    public string Content { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string? UserId { get; set; }
    public bool IsPremium { get; set; }
} 