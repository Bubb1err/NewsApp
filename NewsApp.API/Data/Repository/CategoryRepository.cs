using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository;

[Repository(typeof(Category))]
public class CategoryRepository(ApplicationDbContext context) : BaseRepository<Category>(context)
{
    
}