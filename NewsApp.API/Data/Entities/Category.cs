using NewsApp.API.Data.Entities.Base;

namespace NewsApp.API.Data.Entities;

public class Category : SingleKeyEntity
{
    public string Name { get; set; }
    
    public List<Article> Articles { get; set; }
    
    
    public void AddArticle(Article article) => Articles.Add(article);
        
    public void RemoveArticle(Article article) => Articles.Remove(article);

    
    
}