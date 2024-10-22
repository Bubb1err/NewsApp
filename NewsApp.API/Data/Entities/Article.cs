using NewsApp.API.Data.Entities.Base;

namespace NewsApp.API.Data.Entities
{
    public class Article : SingleKeyEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string SourceUrl { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
