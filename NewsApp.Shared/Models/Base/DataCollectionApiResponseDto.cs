namespace NewsApp.Shared.Models.Base;

public class DataCollectionApiResponseDto<T> : ApiResponseDto
{
    public List<T> Items { get; set; } = new List<T>();
    public int? TotalItemsCount { get; set; }
}