namespace NewsApp.Shared.Models.Dto;

public class CategoryDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public List<ArticleDto> Articles { get; set; }
}