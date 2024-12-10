using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.Shared.Models.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string SourceUrl { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
        
        public List<CommentDto> Comments { get; set; }
        
        public int LikeCount { get; set; }

    }
}
