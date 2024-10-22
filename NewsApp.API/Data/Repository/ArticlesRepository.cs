using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository
{
    public class ArticlesRepository : BaseRepository<Article>
    {
        public ArticlesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
