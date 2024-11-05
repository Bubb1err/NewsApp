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

namespace NewsApp.API.Services
{
    [Service]
    public class ExtractNewestArticlesService
    {
        private readonly UnitOfWork _unitOfWork;

        public ExtractNewestArticlesService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ParseAndSaveNewArticles()
        {
            var articles = await GetArticles();

            var articlesToSave = articles.Select(a => new Article { Title = a.Title, Content = a.Content, PublishDate = a.PublishDate, SourceUrl = a.Link});

            foreach (var article in articlesToSave)
            {
                if (!_unitOfWork.GetRepository<Article>().GetAll().Any(a => a.SourceUrl == article.SourceUrl))
                {
                    try
                    {
                        await _unitOfWork.GetRepository<Article>().AddAsync(article);
                        await _unitOfWork.SaveAsync();
                        Console.WriteLine($"Saved article: {article.Title}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving article: {ex.Message}");
                    }
                }
            }
        }

        private async Task<List<ArticleDto>> GetArticles()
        {
            var articles = await GetArticlesFromRss();

            foreach (var article in articles)
            {
                var content = await ParseContentFromPage(article.Link);
                article.Content = content;
            }

            var sb = new StringBuilder();
            foreach (var article in articles)
            {
                sb.AppendLine(article.Title);
                sb.AppendLine(article.Link);
                sb.AppendLine(article.Content);
                sb.AppendLine(article.PublishDate.ToString());
            }
            Console.WriteLine(sb.ToString());

            return articles;
        }

        private async Task<List<ArticleDto>> GetArticlesFromRss()
        {
            string rssUrl = "https://feeds.bbci.co.uk/news/rss.xml";

            try
            {
                using HttpClient client = new HttpClient();

                string rssContent = await client.GetStringAsync(rssUrl);

                XDocument rssXml = XDocument.Parse(rssContent);

                var articles = rssXml.Descendants("item")
                    .Select(item => new ArticleDto
                    {
                        Title = item.Element("title")?.Value,
                        Link = item.Element("link")?.Value,
                        PublishDate = RssDateConverter.ConvertRssDateToDateTime(item.Element("pubDate")?.Value) ?? DateTime.Now
                    }).ToList();

                return articles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return new List<ArticleDto>();
            }
        }

        private async Task<string> ParseContentFromPage(string url)
        {
            var extractedContent = new List<string>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);

            var headerNode = document.DocumentNode.SelectSingleNode("//h1");
            string header = string.Empty;

            if (headerNode != null)
            {
                header = HttpUtility.HtmlDecode(headerNode.InnerText.Trim());
            }

            //var className = ParserConfigurations[clientKey].ContentContainerClass;
            var className = "fYAfXe";
            var contentBlocks = document.DocumentNode.SelectNodes(string.Format("//*[contains(concat(' ', normalize-space(@class), ' '), ' {0} ') and not(self::script)]", className));

            if (contentBlocks == null)
            {
                return string.Empty;
            }

            var descendants = contentBlocks.Descendants();

            //if (!string.IsNullOrWhiteSpace(ParserConfigurations[clientKey].StopSequence))
            //{
            //    descendants = descendants.Where(node => node.NodeType == HtmlNodeType.Text && !node.InnerText.Contains(ParserConfigurations[clientKey].StopSequence, StringComparison.OrdinalIgnoreCase));
            //}

            descendants = descendants.ToList();

            foreach (var node in descendants)
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    string content = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(content))
                    {
                        extractedContent.Add(HttpUtility.HtmlDecode(content));
                    }
                }
            }

            var sb = new StringBuilder();

            foreach (var content in extractedContent)
            {
                sb.Append(content);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
