namespace NewsApp.Shared.Models.Dto;

public class CreateArticleDto
{
    public string Title { get; set; }
    
    public Guid? CategoryId { get; set; }

    public string Author { get; set; }

    public string AuthorId { get; set; }

    public string Content { get; set; }
    public bool IsPremium { get; set; }
}