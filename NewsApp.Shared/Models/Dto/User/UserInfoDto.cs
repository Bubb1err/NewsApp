
using NewsApp.Shared.Models.Base;

namespace NewsApp.Shared.Models.Dto.User
{
    public class UserInfoDto : ApiResponseDto
    {
        public string Id { get; set; }
        
        public string Email { get; set; } = string.Empty;

        public Dictionary<string, string> Claims { get; set; } = [];
    }
}
