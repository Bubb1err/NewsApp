using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.Shared.Models.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public string AuthorId { get; set; }
        public string Author { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public string SourceUrl { get; set; } = string.Empty;
        public CategoryDto? Category { get; set; }
        public bool IsPremium { get; set; }
    }
}
