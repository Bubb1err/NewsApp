using Microsoft.Extensions.Caching.Memory;

namespace NewsApp.API.Data.Repository.Base
{
    public class UnitOfWork
    {
        private ArticlesRepository _articlesRepository;
        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext => _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ArticlesRepository Articles =>
            _articlesRepository ?? (_articlesRepository = new ArticlesRepository(_dbContext));

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
