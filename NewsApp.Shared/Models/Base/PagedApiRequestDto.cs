namespace NewsApp.Shared.Models.Base;

public class PagedApiRequestDto
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
}