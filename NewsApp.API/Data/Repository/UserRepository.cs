using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context);