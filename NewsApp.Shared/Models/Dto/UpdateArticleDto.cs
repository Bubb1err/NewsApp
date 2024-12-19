namespace NewsApp.Shared.Models.Dto;

public class UpdateArticleDto
{
    public Guid ArticleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? UserId { get; set; }
} 