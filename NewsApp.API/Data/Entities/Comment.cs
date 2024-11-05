using System.ComponentModel.DataAnnotations;
using NewsApp.API.Data.Entities.Base;

namespace NewsApp.API.Data.Entities;

public class Comment : SingleKeyEntity
{
    [Required]
    public string Content { get; set; } = string.Empty;

    public int Likes { get; set; }
    
    public string UserId { get; set; }
    public User User { get; set; }
  
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }

    
}