using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryList
{
    public record GetCategoriesQuery : PagedRequest, IQuery<PagedList<CategoryListItem>>
    {
        public bool? IsActive { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int? Level { get; set; }
        public bool? HasProducts { get; set; }

        public Guid[]? Ids { get; set; }

        public string? SearchIn { get; set; } // "name", "description"
    }
}
