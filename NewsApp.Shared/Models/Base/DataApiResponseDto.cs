namespace NewsApp.Shared.Models.Base;

public class DataApiResponseDto<T> : ApiResponseDto
{
    
    public T Item { get; set; }
}