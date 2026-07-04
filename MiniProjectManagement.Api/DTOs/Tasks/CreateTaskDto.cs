using System.ComponentModel.DataAnnotations;

namespace MiniProjectManagement.Api.DTOs.Tasks;

public class CreateTaskDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }
}