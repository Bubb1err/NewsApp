namespace NewsApp.Shared.Models.Dto.Coment;

public class CommentDto
{
 
    public string Content { get; set; } = string.Empty;
    public Guid ArticleId { get; set; }
}