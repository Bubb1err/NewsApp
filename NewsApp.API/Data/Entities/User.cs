
using Microsoft.AspNetCore.Identity;
using NewsApp.Shared.Constants;

namespace NewsApp.API.Data.Entities;

public class User : IdentityUser
{
    public string? Name { get; set; }
    
   public UserState State { get; set; } = UserState.Unknown;
    
    
    public virtual List<Comment> Comments { get; set; } = [];
    

    public  List<Guid> Liked { get; set; } = [];

    public  List<Guid> Saved { get; set; } = [];

    public void AddLike(Guid liked)
    {
        Liked.Add(liked);
    }
    
    public void RemoveLike(Guid liked)
    {
        Liked.Remove(liked);
    }
    
    
    public void AddSaved(Guid saved)
    {
        Console.WriteLine(saved);
        Saved.Add(saved);
    }
    
    public void RemoveSaved(Guid saved)
    {
        Saved.Remove(saved);
    }

    public void ChangeUserState( UserState newState )
    {
        State = newState;
    }
    public void AwaitingState() {
        State = UserState.Awaiting;
    }


}