namespace Zord.Result
{
    /// <summary>
    /// Lookup data entries with pagination
    /// </summary>
    public class PageLookup
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}