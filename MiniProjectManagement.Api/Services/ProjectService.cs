using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Data;
using MiniProjectManagement.Api.DTOs.Projects;
using MiniProjectManagement.Api.Helpers;
using MiniProjectManagement.Api.Models;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(AppDbContext dbContext, ILogger<ProjectService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<ProjectResponseDto>> GetAllProjectsAsync()
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
                
            })
            .ToListAsync();
    }

    public async Task<ServiceResult<ProjectResponseDto>> GetProjectByIdAsync(int id)
    {
        var project = await _dbContext.Projects
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (project is null)
        {
            _logger.LogWarning("Project not found. ProjectId: {ProjectId}", id);
            return ServiceResult<ProjectResponseDto>.NotFound("Project not found");
        }
        
        return ServiceResult<ProjectResponseDto>.Success(project);
    }

    public async Task<ServiceResult<ProjectResponseDto>> CreateProjectAsync(CreateProjectDto dto)
    {
        var projectNameExists = await _dbContext.Projects.AnyAsync(p => p.Name == dto.Name);

        if (projectNameExists)
        {
            _logger.LogWarning("project creation failed. Dublicate project name: {ProjectName}", dto.Name);
            
            return ServiceResult<ProjectResponseDto>.Conflict("Project name already exists");
        }
        
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };
        
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Project created. ProjectId: {ProjectId}", project.Id);

        var response = new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
        
        return ServiceResult<ProjectResponseDto>.Success(response);
    }

    public async Task<ServiceResult<bool>> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
        {
            _logger.LogWarning("Project not found. ProjectId: {ProjectId}", id);
            
            return ServiceResult<bool>.NotFound("Project not found");
        }
        
        var projectNameExists = await _dbContext.Projects
            .AnyAsync(p => p.Name == updateProjectDto.Name && p.Id != id);

        if (projectNameExists)
        {
            _logger.LogWarning("project name already exists. ProjectId: {ProjectId}", id);
            
            return ServiceResult<bool>.NotFound("Project name already exists");
        }
        
        project.Name = updateProjectDto.Name;
        project.Description = updateProjectDto.Description;
        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Project updated. ProjectId: {ProjectId}", id);
        
        return ServiceResult<bool>.Success(true);
    }

    public async Task<ServiceResult<bool>> DeleteProjectAsync(int id)
    {
        var project = await _dbContext.Projects.FindAsync(id);

        if (project is null)
        {
            _logger.LogWarning("Project delete failed. Project not found. ProjectId: {ProjectId}", id);
            return ServiceResult<bool>.NotFound("Project not found");
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Project deleted. ProjectId: {ProjectId}", id);
        
        return ServiceResult<bool>.Success(true);
    }
}