namespace ECommerce.Infrastructure.Logging;

public sealed class SerilogLogger(Serilog.ILogger logger) : Application.Common.Logging.ILogger
{
    public void LogInformation(string message, params object[] args) =>
        logger.Information(message, args);

    public void LogWarning(string message, params object[] args) =>
        logger.Warning(message, args);

    public void LogError(string message, params object[] args) =>
        logger.Error(message, args);

    public void LogDebug(string message, params object[] args) =>
        logger.Debug(message, args);

    public void LogCritical(string message, params object[] args) =>
        logger.Fatal(message, args);
}