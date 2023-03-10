namespace Zord.Extensions.Logging
{
    public class Logger : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public Logger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void Information(string message, params object?[] args)
        {
            _logger.Information(message, args);
        }

        public void Warning(string message, params object?[] args)
        {
            _logger.Warning(message, args);
        }

        public void Error(string message, params object?[] args)
        {
            _logger.Error(message, args);
        }

        public void Debug(string message, params object?[] args)
        {
            _logger.Debug(message, args);
        }
    }
}
