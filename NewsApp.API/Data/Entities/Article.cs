using NewsApp.API.Data.Entities.Base;

namespace NewsApp.API.Data.Entities
{
    public class Article : SingleKeyEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string SourceUrl { get; set; }

        public DateTime PublishDate { get; set; }
        
        public List<Comment> Comments { get; set; } = [];
        
        public int LikeCount { get; set; }
        
        public int SavedCount { get; set; }
        
        public void AddLike() => LikeCount++;
        
        public void RemoveLike() => LikeCount--;
        
        
        
        public void AddSaved() => SavedCount++;
        
        public void RemoveSaved() => SavedCount--;


    }
}
