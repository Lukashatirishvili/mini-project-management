using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Data;
using MiniProjectManagement.Api.DTOs.Projects;
using MiniProjectManagement.Api.Models;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _dbContext;

    public ProjectService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
    {
        return await _dbContext.Projects
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
    }

    public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };
        
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task<bool> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
    {
        var project = await _dbContext.Projects.FindAsync(id);

        if (project is null)
        {
            return false;
        }
        
        project.Name = updateProjectDto.Name;
        project.Description = updateProjectDto.Description;
        
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _dbContext.Projects.FindAsync(id);

        if (project is null)
        {
            return false;
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}