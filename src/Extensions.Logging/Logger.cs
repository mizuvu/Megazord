using Zord.Core;

namespace Zord.Extensions.Logging;

public class Logger : ILogger
{
    private readonly Serilog.ILogger _logger;

    public Logger(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public virtual void Information(string message, params object?[] args)
    {
        _logger.Information(message, args);
    }

    public virtual void Warning(string message, params object?[] args)
    {
        _logger.Warning(message, args);
    }

    public virtual void Error(string message, params object?[] args)
    {
        _logger.Error(message, args);
    }

    public virtual void Debug(string message, params object?[] args)
    {
        _logger.Debug(message, args);
    }
}
