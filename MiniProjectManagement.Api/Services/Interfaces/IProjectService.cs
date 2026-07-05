using MiniProjectManagement.Api.DTOs.Projects;
using MiniProjectManagement.Api.Helpers;

namespace MiniProjectManagement.Api.Services.Interfaces;

public interface IProjectService
{
    Task<List<ProjectResponseDto>> GetAllProjectsAsync();
    Task<ServiceResult<ProjectResponseDto>> GetProjectByIdAsync(int id);
    Task<ServiceResult<ProjectResponseDto>> CreateProjectAsync(CreateProjectDto createProjectDto);
    Task<ServiceResult<bool>> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
    Task<ServiceResult<bool>> DeleteProjectAsync(int id);
}