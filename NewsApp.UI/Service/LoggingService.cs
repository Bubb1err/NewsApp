using Microsoft.Extensions.Logging;

namespace NewsApp.UI.Service;

public interface ILoggingService
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception = null);
    void LogDebug(string message);
}

public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public void LogInfo(string message)
    {
        _logger.LogInformation("[UI] {Time} - {Message}", DateTime.UtcNow, message);
        Console.WriteLine($"[INFO] {DateTime.UtcNow} - {message}");
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning("[UI] {Time} - {Message}", DateTime.UtcNow, message);
        Console.WriteLine($"[WARN] {DateTime.UtcNow} - {message}");
    }

    public void LogError(string message, Exception? exception = null)
    {
        if (exception != null)
        {
            _logger.LogError(exception, "[UI] {Time} - {Message}", DateTime.UtcNow, message);
            Console.WriteLine($"[ERROR] {DateTime.UtcNow} - {message}\nException: {exception.Message}\nStackTrace: {exception.StackTrace}");
        }
        else
        {
            _logger.LogError("[UI] {Time} - {Message}", DateTime.UtcNow, message);
            Console.WriteLine($"[ERROR] {DateTime.UtcNow} - {message}");
        }
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug("[UI] {Time} - {Message}", DateTime.UtcNow, message);
        Console.WriteLine($"[DEBUG] {DateTime.UtcNow} - {message}");
    }
} 