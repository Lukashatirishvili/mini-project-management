using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Data;
using MiniProjectManagement.Api.DTOs.Common;
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


    public async Task<PagedResponseDto<TaskResponseDto>> GetAllTasksAsync(TaskQueryParameters queryParameters)
    {
        var query = _context.ProjectTasks
            .AsNoTracking()
            .AsQueryable();

        if (queryParameters.ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == queryParameters.ProjectId.Value);
        }

        if (queryParameters.Status.HasValue)
        {
            query = query.Where(t => t.Status == queryParameters.Status.Value);
        }

        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            var search = queryParameters.Search.Trim();

            query = query.Where(t =>
                t.Description != null && t.Description.Contains(search) || t.Title.Contains(search));
        }
        
        var totalCount = await query.CountAsync();

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
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

        return new PagedResponseDto<TaskResponseDto>
        {
            Page = queryParameters.Page,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize),
            Data = tasks
        };

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