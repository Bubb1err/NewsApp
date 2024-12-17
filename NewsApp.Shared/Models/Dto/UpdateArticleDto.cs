namespace NewsApp.Shared.Models.Dto;

public class UpdateArticleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public string? UserId { get; set; }
} 