using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Data.Repository;

[Repository(typeof(Comment))]
public class CommentRepository(ApplicationDbContext context) : BaseRepository<Comment>(context);
