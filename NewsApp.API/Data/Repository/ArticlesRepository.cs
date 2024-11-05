using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository
{
    [Repository(typeof(Article))]
    public class ArticlesRepository : BaseRepository<Article>
    {
        public ArticlesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
