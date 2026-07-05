using Microsoft.AspNetCore.Mvc;
using MiniProjectManagement.Api.DTOs.Common;
using MiniProjectManagement.Api.DTOs.Tasks;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : Controller
{
    private readonly IProjectTaskService _projectTaskService;

    public TasksController(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<TaskResponseDto>>> GetTasks(
        [FromQuery] TaskQueryParameters parameters)
    {
        var tasks = await _projectTaskService.GetAllTasksAsync(parameters);
        
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
    {
        var tasks = await _projectTaskService.GetTaskByIdAsync(id);

        if (tasks == null)
        {
            return NotFound();
        }
        
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)
    {
        var task = await _projectTaskService.CreateTaskAsync(dto);

        if (task is null)
        {
            return  BadRequest("Project Does not exist");
        }
        
        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTask(int id, UpdateTaskDto dto)
    {
        var updated = await _projectTaskService.UpdateTaskAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var deleted = await _projectTaskService.DeleteTaskAsync(id);
        
        if  (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}