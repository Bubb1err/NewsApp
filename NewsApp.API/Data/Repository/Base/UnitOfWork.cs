

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
            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<RepositoryAttribute>() != null);

            foreach (var repositoryType in repositoryTypes)
            {
                var attribute = repositoryType.GetCustomAttribute<RepositoryAttribute>();
                var entityType = attribute.EntityType;

                var repositoryInstance = Activator.CreateInstance(repositoryType, _dbContext);

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
}