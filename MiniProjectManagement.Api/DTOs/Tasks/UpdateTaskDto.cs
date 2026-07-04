using System.ComponentModel.DataAnnotations;
using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.DTOs.Tasks;

public class UpdateTaskDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public ProjectTaskStatus Status { get; set; }
}