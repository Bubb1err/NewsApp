using System.Net;

namespace NewsApp.Shared.Models.Base
{
    public class ApiResponseDto
    {
        public ApiResponseDto()
        {

        }

        public ApiResponseDto(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(ErrorCode);
            }
        }

        public bool AlreadyExists { get => ErrorCode == HttpStatusCode.Conflict.ToString(); }
    }
}
