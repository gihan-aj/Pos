using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Shared.Abstractions
{
    public class PagedList<T>
    {
        public List<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasNextPage => Page * PageSize < TotalCount;

        public bool HasPreviousPage => Page > 1;

        public PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var count = await EntityFrameworkQueryableExtensions.CountAsync(source, cancellationToken);
            var items = await EntityFrameworkQueryableExtensions.ToListAsync(
                source.Skip((page - 1) * pageSize).Take(pageSize), cancellationToken);

            return new PagedList<T>(items, page, pageSize, count);
        }
    }
}
