namespace Zord.Extensions.Logging
{
    public interface ILogger
    {
        void Information(string message, params object[] args);

        void Warning(string message, params object[] args);

        void Error(string message, params object[] args);

        void Debug(string message, params object[] args);
    }
}