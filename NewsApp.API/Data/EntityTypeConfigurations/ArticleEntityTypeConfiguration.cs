using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsApp.API.Data.Entities;

namespace NewsApp.API.Data.EntityTypeConfigurations
{
    public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired();

            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.PublishDate).IsRequired();

            
            builder.Property(x => x.Author);
            
            builder.Property(x => x.SourceUrl).IsRequired();

            builder.HasIndex(x => x.SourceUrl).IsUnique();
            
            builder.Property(x=>x.LikeCount);
            
            builder.HasMany(a => a.Comments)
                .WithOne(c => c.Article)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(c => c.Category)
                .WithMany(a => a.Articles)
                .HasForeignKey(c => c.CategoryId);

        }
    }
}
