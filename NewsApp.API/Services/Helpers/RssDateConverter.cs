using System.Globalization;

namespace NewsApp.API.Services.Helpers
{
    public static class RssDateConverter
    {
        public static DateTime? ConvertRssDateToDateTime(string publishDate)
        {
            if (string.IsNullOrWhiteSpace(publishDate))
            {
                return null;
            }

            string[] formats =
            {
                "ddd, dd MMM yyyy HH:mm:ss zzz",
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                "ddd, dd MMM yyyy HH:mm:ss",
                "dd MMM yyyy HH:mm:ss zzz",
                "dd MMM yyyy HH:mm:ss"
            };

            if (DateTime.TryParseExact(publishDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime result))
            {
                return result;
            }

            try
            {
                return DateTime.Parse(publishDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing date: {ex.Message}");
                return null;
            }
        }
    }
}
