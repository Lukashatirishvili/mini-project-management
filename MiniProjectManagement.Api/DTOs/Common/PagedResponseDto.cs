namespace MiniProjectManagement.Api.DTOs.Common;

public class PagedResponseDto<T>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public List<T> Data { get; set; } = new();
}