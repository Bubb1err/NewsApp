namespace NewsApp.Shared.Models.Dto.User;

public class UpdateDto
{
    public string userId { get; set; }
    public Guid articleId { get; set; }
    public bool Value { get; set; }
    
}