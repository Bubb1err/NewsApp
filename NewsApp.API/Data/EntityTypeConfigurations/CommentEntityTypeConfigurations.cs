using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsApp.API.Data.Entities;

namespace NewsApp.API.Data.EntityTypeConfigurations;

public class CommentEntityTypeConfigurations : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
    
        builder.HasOne(f => f.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.Article)
            .WithMany(a => a.Comments) 
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
    
}