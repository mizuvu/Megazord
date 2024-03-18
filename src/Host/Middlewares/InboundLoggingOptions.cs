namespace Zord.Host.Middlewares
{
    public class InboundLoggingOptions
    {
        public bool Enable { get; set; }
        public bool IncludeResponse { get; set; }
        public List<string>? ExcludePaths { get; set; }
    }
}
