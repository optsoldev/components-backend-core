namespace Optsol.Components.Domain.Pagination
{
    public interface ISearchRequest<TSearch> where TSearch : class
    {
        uint Page { get; set; }

        uint? PageSize { get; set; }

        TSearch Search { get; set; }
    }
}