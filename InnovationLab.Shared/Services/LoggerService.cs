using InnovationLab.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace InnovationLab.Shared.Services;

public class LoggerService(ILogger logger) : ILoggerService
{
    private readonly ILogger _logger = logger;

    public void LogInfo(string message)
    {
        _logger.LogInformation("{message}", message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning("{message}", message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        _logger.LogError(ex, "{message}", message);
    }
}