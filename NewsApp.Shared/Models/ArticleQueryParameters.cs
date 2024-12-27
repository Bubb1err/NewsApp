namespace NewsApp.Shared.Models
{
    public class ArticleQueryParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        
        public int PageNumber { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        
        public string? CategoryName { get; set; }
        
        public string? SortBy { get; set; } 
        public bool Descending { get; set; } = true;
        
        public string? SearchTerm { get; set; }
    }
} 