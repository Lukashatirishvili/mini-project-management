using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.DTOs.Tasks;

public class TaskResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ProjectTaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;
}