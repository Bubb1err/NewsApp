using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.Shared.Models
{
    public class ArticleDto
    {
        public string Title { get; set; }

        public string SourceUrl { get; set; }

        public string Content { get; set; }

        public Guid AuthorId { get; set; }
        public string? Author { get; set; }
        public DateTime PublishDate { get; set; }
        
        public List<CommentDto> Comments { get; set; }
        
        public CategoryDto Category { get; set; }

        public bool IsPremium { get; set; }

        public int LikeCount { get; set; }
        
        public int SavedCount { get; set; }

        
    }
    
    
}
