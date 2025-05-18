namespace ECommerce.SharedKernel.Logging;

public sealed record LoggingOptions
{
    public string MinimumLevel { get; init; } = "Information";
    public string FilePath { get; init; } = "logs/log.txt";
    public string SeqUrl { get; init; } = "http://localhost:5341";
    public string OutputTemplate { get; init; } =
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
    public bool EnableConsole { get; init; } = true;
    public bool EnableFile { get; init; } = true;
}