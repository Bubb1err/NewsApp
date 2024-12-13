using NewsApp.API.Data.Entities.Base;

namespace NewsApp.API.Data.Entities
{
    public class Article : SingleKeyEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string SourceUrl { get; set; }
        
        public Guid CategoryId { get; set; }
        
        public Category Category { get; set; }
        
        public string AuthorId { get; set; }
        
        public string Author { get; set; }

        public DateTime PublishDate { get; set; }
        
        public List<Comment> Comments { get; set; } = [];
        
        public int LikeCount { get; set; }
        
        public int SavedCount { get; set; }
        
        public void AddLike() => LikeCount++;
        
        public void RemoveLike() => LikeCount--;
        
        
        
        public void AddSaved() => SavedCount++;
        
        public void RemoveSaved() => SavedCount--;

        public void Update(string title, string content)
        {
            if (Title != title)
            {
                Title = title;

            }

            if (Content != content)
            {
                Content = content;

            }

            
        }


    }
}
