using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.API.Services.Helpers;
using NewsApp.Shared.Models;
using System.Text;
using System.Web;
using System.Xml.Linq;
using NewsApp.API.Atributes;
using Microsoft.Extensions.Logging;

namespace NewsApp.API.Services
{
    [Service]
    public class ExtractNewestArticlesService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<ExtractNewestArticlesService> _logger;
        private readonly CategoryInitializerService _categoryInitializer;

        private readonly Dictionary<string, string> _rssSources = new()
        {
            { "BBC", "https://feeds.bbci.co.uk/news/rss.xml" },
            {"CNN","http://rss.cnn.com/rss/edition_world.rss"},
            {"FoxNews","https://moxie.foxnews.com/google-publisher/latest.xml"}
        };

        public ExtractNewestArticlesService(
            UnitOfWork unitOfWork, 
            ILogger<ExtractNewestArticlesService> logger,
            CategoryInitializerService categoryInitializer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _categoryInitializer = categoryInitializer;
        }

        public async Task ParseAndSaveNewArticles()
        {
            _logger.LogInformation("Starting to parse and save new articles");
            var articles = await GetArticles();
            _logger.LogInformation($"Found {articles.Count} articles to process");

            foreach (var article in articles)
            {
                try 
                {
                    var exists = await _unitOfWork.GetRepository<Article>()
                        .GetAll()
                        .AnyAsync(a => a.SourceUrl == article.SourceUrl);

                    if (!exists)
                    {
                        var category = await _categoryInitializer.GetCategoryForSource(article.Author);
                        if (category != null)
                        {
                            var trackedCategory = await _unitOfWork.GetRepository<Category>()
                                .GetAll()
                                .FirstOrDefaultAsync(c => c.Id == category.Id);
                            category = trackedCategory;
                        }

                        var newArticle = new Article 
                        { 
                            Title = article.Title, 
                            Content = article.Content, 
                            PublishDate = article.PublishDate, 
                            Author = article.Author,
                            SourceUrl = article.SourceUrl,
                            CategoryId = category?.Id
                        };

                        await _unitOfWork.GetRepository<Article>().AddAsync(newArticle);
                        await _unitOfWork.SaveAsync();
                        
                        _logger.LogInformation($"Saved new article: {article.Title} | Category: {category?.Name ?? "None"}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error saving article: {article.Title}");
                }
            }
        }

        private async Task<List<ArticleDto>> GetArticles()
        {
            _logger.LogInformation("Starting to fetch articles from RSS");
            var articles = await GetArticlesFromRss();

            foreach (var article in articles)
            {
                try
                {
                    var content = await ParseContentFromPage(article.SourceUrl);
                    article.Content = content;
                    _logger.LogInformation($"Successfully parsed content for article: {article.Title}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to parse content for article: {article.Title} | URL: {article.SourceUrl}");
                }
            }

            return articles;
        }

        private async Task<List<ArticleDto>> GetArticlesFromRss()
        {
            _logger.LogInformation("Fetching articles from RSS feeds");
            var allArticles = new List<ArticleDto>();

            foreach (var source in _rssSources)
            {
                try
                {
                    _logger.LogInformation($"Fetching articles from {source.Key}");
                    using var client = new HttpClient();
                    var rssContent = await client.GetStringAsync(source.Value);
                    var rssXml = XDocument.Parse(rssContent);

                    var articles = rssXml.Descendants("item")
                        .Select(item => new ArticleDto
                        {
                            Title = item.Element("title")?.Value,
                            SourceUrl = item.Element("link")?.Value,
                            PublishDate = RssDateConverter.ConvertRssDateToDateTime(item.Element("pubDate")?.Value) ?? DateTime.Now,
                            Author = source.Key,
                            IsPremium = false,
                            Content = item.Element("description")?.Value ?? string.Empty
                        }).ToList();

                    _logger.LogInformation($"Successfully fetched {articles.Count} articles from {source.Key}");
                    allArticles.AddRange(articles);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to fetch articles from {source.Key}");
                }
            }

            _logger.LogInformation($"Total articles fetched from all sources: {allArticles.Count}");
            return allArticles;
        }

        private async Task<string> ParseContentFromPage(string url)
        {
            _logger.LogInformation($"Starting to parse content from URL: {url}");
            try
            {
                var extractedContent = new List<string>();
                var web = new HtmlWeb();
                var document = web.Load(url);
                
                var contentNode = document.DocumentNode.SelectSingleNode("//article");
                if (contentNode == null)
                {
                    _logger.LogWarning($"No content found using selector for URL: {url}");
                    return string.Empty;
                }

                var textNodes = contentNode.DescendantsAndSelf()
                    .Where(n => n.NodeType == HtmlNodeType.Text &&
                               !n.ParentNode.Name.In("script", "style", "nav", "header", "footer") &&
                               !string.IsNullOrWhiteSpace(n.InnerText));

                foreach (var node in textNodes)
                {
                    var text = HttpUtility.HtmlDecode(node.InnerText.Trim());
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        extractedContent.Add(text);
                    }
                }

                var result = string.Join(" ", extractedContent);
                _logger.LogInformation($"Successfully parsed content from URL: {url}, Content length: {result.Length}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error parsing content from URL: {url}");
                return string.Empty;
            }
        }
    }

    public static class StringExtensions
    {
        public static bool In(this string str, params string[] values)
        {
            return values.Contains(str, StringComparer.OrdinalIgnoreCase);
        }
    }
}
