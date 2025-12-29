using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryList
{
    public record GetCategoriesQuery : PagedRequest
    {
        [FromQuery]
        public bool? IsActive { get; set; }
        [FromQuery]
        public Guid? ParentCategoryId { get; set; }
        [FromQuery]
        public int? Level { get; set; }
        [FromQuery]
        public bool? HasProducts { get; set; }

        [FromQuery]
        public Guid[]? Ids { get; set; }

        [FromQuery]
        public string? SearchIn { get; set; } // "name", "description"
    }
}
