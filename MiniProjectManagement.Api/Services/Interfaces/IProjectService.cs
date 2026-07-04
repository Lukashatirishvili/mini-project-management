using MiniProjectManagement.Api.DTOs.Projects;

namespace MiniProjectManagement.Api.Services.Interfaces;

public interface IProjectService
{
    Task<List<ProjectResponseDto>> GetAllProjectsAsync();
    Task<ProjectResponseDto?> GetProjectByIdAsync(int id);
    Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto);
    Task<bool> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
    Task<bool> DeleteProjectAsync(int id);
}