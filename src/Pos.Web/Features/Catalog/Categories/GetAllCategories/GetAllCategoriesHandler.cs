using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetAllCategories
{
    public class GetAllCategoriesHandler : IQueryHandler<GetAllCategoriesQuery, List<CategorySummaryItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetAllCategoriesHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<CategorySummaryItem>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Categories
                .AsNoTracking()
                .AsQueryable();

            if (request.OnlyLeafCategories != null && request.OnlyLeafCategories == true)
            {
                query = query.Where(c => c.SubCategories.Count() == 0);
            }

            var categories = await query
                .Where(c => c.IsActive)
                .ToListAsync(cancellationToken);

            var response = categories
                .Select(c => new CategorySummaryItem(c.Id, c.Name, c.NamePath, c.Level, c.DisplayOrder))
                .ToList();

            return response;
        }
    }
}
