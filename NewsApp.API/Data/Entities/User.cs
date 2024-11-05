
using Microsoft.AspNetCore.Identity;

namespace NewsApp.API.Data.Entities;

public class User : IdentityUser
{
    public string? Name { get; set; }
    
    
    public virtual List<Comment> Comments { get; set; } = [];
    
}