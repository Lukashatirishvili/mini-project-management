using System.ComponentModel.DataAnnotations;
using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.DTOs.Tasks;

public class TaskQueryParameters
{
    [Range(1, int.MaxValue)]
    public int? ProjectId { get; set; }

    public ProjectTaskStatus? Status { get; set; }

    [MaxLength(100)]
    public string? Search { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}