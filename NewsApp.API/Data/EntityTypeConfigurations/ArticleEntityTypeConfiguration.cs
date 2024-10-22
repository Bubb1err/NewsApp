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

            builder.Property(x => x.SourceUrl).IsRequired();

            builder.HasIndex(x => x.SourceUrl).IsUnique();
        }
    }
}
