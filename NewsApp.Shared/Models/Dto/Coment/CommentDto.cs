namespace NewsApp.Shared.Models.Dto.Coment;

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public string UserName { get; set; }
    public string Content { get; set; } = string.Empty;
    
}