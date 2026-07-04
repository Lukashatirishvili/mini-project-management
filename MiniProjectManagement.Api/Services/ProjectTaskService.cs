using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Data;
using MiniProjectManagement.Api.DTOs.Tasks;
using MiniProjectManagement.Api.Models;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly AppDbContext _context;

    public ProjectTaskService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TaskResponseDto>> GetAllTasksAsync()
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                ProjectId = t.ProjectId,
                ProjectName = t.Project.Name
                
            })
            .ToListAsync();
    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(int id)
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                ProjectId = t.ProjectId,
                ProjectName = t.Project.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TaskResponseDto?> CreateTaskAsync(CreateTaskDto dto)
    {
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);

        if (!projectExists)
        {
            return null;
        }

        var task = new ProjectTask
        {
            Title = dto.Title,
            Description = dto.Description,
            ProjectId = dto.ProjectId,
            Status = ProjectTaskStatus.Todo,
            CreatedAt = DateTime.UtcNow,
        };
        
        _context.ProjectTasks.Add(task);
        await _context.SaveChangesAsync();
        
        return await GetTaskByIdAsync(task.Id);

    }

    public async Task<bool> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return false;
        }
        
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _context.ProjectTasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }
        
        _context.ProjectTasks.Remove(task);
        await _context.SaveChangesAsync();
        
        return true;
    }
}