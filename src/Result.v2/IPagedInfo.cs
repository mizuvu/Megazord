namespace Zord.Result
{
    public interface IPagedInfo
    {
        bool HasNextPage { get; }

        bool HasPreviousPage { get; }

        int Page { get; }

        int PageSize { get; }

        int TotalPages { get; }

        int TotalRecords { get; }
    }
}