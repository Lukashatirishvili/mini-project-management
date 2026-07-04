using MiniProjectManagement.Api.DTOs.Tasks;

namespace MiniProjectManagement.Api.Services.Interfaces;

public interface IProjectTaskService
{
    Task<List<TaskResponseDto>> GetAllTasksAsync();
    Task<TaskResponseDto?> GetTaskByIdAsync(int id);
    Task<TaskResponseDto?> CreateTaskAsync(CreateTaskDto dto);
    Task<bool> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(int id);
}