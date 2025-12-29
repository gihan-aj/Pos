using Microsoft.AspNetCore.Mvc;

namespace Pos.Web.Shared.Abstractions
{
    public record PagedRequest
    {
        [FromQuery]
        public int Page { get; set; } = 1;
        [FromQuery]
        public int PageSize { get; set; } = 20;
        [FromQuery]
        public string? Search { get; set; }
        [FromQuery]
        public string? SortBy { get; set; }
        [FromQuery]
        public string? SortOrder { get; set; } = "asc";
    }
}
