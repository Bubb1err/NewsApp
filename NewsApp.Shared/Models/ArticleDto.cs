using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.Shared.Models
{
    public class ArticleDto
    {
        public string Title { get; set; }

        public string SourceUrl { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
        
        public List<CommentDto> Comments { get; set; }
        
        public int LikeCount { get; set; }
    }
}
