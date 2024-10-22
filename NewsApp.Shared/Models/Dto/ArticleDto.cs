namespace NewsApp.Shared.Models.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
