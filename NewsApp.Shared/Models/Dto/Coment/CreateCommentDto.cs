namespace NewsApp.Shared.Models.Dto.Coment;

public class CreateCommentDto
{
    public string Content { get; set; } 
    public Guid UserId { get; set; } 
    public Guid ArticleId { get; set; } 
}