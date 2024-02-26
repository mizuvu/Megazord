namespace Zord.Result
{
    public interface IPagedInfo : IPage
    {
        int TotalPages { get; }

        int TotalRecords { get; }

        bool HasNextPage { get; }

        bool HasPreviousPage { get; }
    }
}