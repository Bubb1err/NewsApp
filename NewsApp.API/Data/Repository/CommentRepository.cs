using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository;

public class CommentRepository(ApplicationDbContext context) : BaseRepository<Comment>(context);
