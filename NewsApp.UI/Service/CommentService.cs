using System.Net.Http.Headers;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.UI.Service;

public class CommentService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenProvider;

    public Guid _currentarticleId { get; set; }

    public CommentService(HttpClient httpClient, ITokenService tokenProvider)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

        _httpClient = httpClient;
    }


    public async Task<List<CommentDto>> GetCommentsDto(Guid articleId)
    {
        var comments =
            await _httpClient.GetFromJsonAsync<DataApiResponseDto<List<CommentDto>>>(
                $"Comment/by-article/{articleId}");

        return comments.Item;
    }

    public async Task<bool> CreateComment(CreateCommentDto comment)
    {
        var token = await _tokenProvider.GetTokenAsync();


        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        Console.WriteLine(_httpClient.DefaultRequestHeaders.Authorization.ToString());
        var response = await _httpClient.PostAsJsonAsync("Comment", comment);
        Console.WriteLine(response.StatusCode);
        return response.IsSuccessStatusCode;
    }
}