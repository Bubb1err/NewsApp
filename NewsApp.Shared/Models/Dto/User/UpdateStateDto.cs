using NewsApp.Shared.Constants;

namespace NewsApp.Shared.Models.Dto.User;

public class UpdateStateDto

{ public string UserId { get; set; }
    public UserState State { get; set; }
    
}