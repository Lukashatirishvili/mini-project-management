using MiniProjectManagement.Api.DTOs.Common;
using MiniProjectManagement.Api.DTOs.Tasks;
using MiniProjectManagement.Api.Helpers;

namespace MiniProjectManagement.Api.Services.Interfaces;

public interface IProjectTaskService
{
    Task<PagedResponseDto<TaskResponseDto>> GetTasksAsync(TaskQueryParameters queryParameters);

    Task<ServiceResult<TaskResponseDto>> GetTaskByIdAsync(int id);

    Task<ServiceResult<TaskResponseDto>> CreateTaskAsync(CreateTaskDto dto);

    Task<ServiceResult<bool>> UpdateTaskAsync(int id, UpdateTaskDto dto);

    Task<ServiceResult<bool>> DeleteTaskAsync(int id);
}