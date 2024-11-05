using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository;

[Repository(typeof(User))]

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context);