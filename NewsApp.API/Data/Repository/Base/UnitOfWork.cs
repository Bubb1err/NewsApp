

using System.Reflection;
using NewsApp.API.Atributes;

namespace NewsApp.API.Data.Repository.Base
{
    [Service]
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
            RegisterRepositories();
        }

        private void RegisterRepositories()
        {
            // Get all types in the current assembly
            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<RepositoryAttribute>() != null);

            foreach (var repositoryType in repositoryTypes)
            {
                // Get the entity type from the attribute
                var attribute = repositoryType.GetCustomAttribute<RepositoryAttribute>();
                var entityType = attribute.EntityType;

                // Create an instance of the repository
                var repositoryInstance = Activator.CreateInstance(repositoryType, _dbContext);

                // Add it to the dictionary
                _repositories.Add(entityType, repositoryInstance);
            }
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var entityType = typeof(T);
            if (_repositories.TryGetValue(entityType, out var repository))
            {
                return (IRepository<T>)repository;
            }

            throw new InvalidOperationException($"Repository for {entityType.Name} not found.");
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
    
    
    
    /*[Service]
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
            _commentRepository ??= new CommentRepository(_dbContext);
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
    }*/
}
