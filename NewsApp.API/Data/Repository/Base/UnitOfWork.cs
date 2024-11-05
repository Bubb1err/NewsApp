
namespace NewsApp.API.Data.Repository.Base
{
    public class UnitOfWork
    {
        private ArticlesRepository _articlesRepository;
        private UserRepository _userRepository;
        private CommentRepository _commentRepository;
        
        
        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext => _dbContext;


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public CommentRepository Comment =>
            _commentRepository ?? (_commentRepository = new CommentRepository(_dbContext));
        public UserRepository Users => 
            _userRepository ?? (_userRepository = new UserRepository(_dbContext));
        
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
