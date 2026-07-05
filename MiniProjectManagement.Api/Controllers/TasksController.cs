using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using MiniProjectManagement.Api.DTOs.Common;
using MiniProjectManagement.Api.DTOs.Tasks;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : BaseApiController
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
        var tasks = await _projectTaskService.GetTasksAsync(parameters);
        
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
    {
        var result = await _projectTaskService.GetTaskByIdAsync(id);

        if (!result.Succeeded)
        {
            return HandleServiceError<TaskResponseDto>(result);
        }
        
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)
    {
        var result = await _projectTaskService.CreateTaskAsync(dto);

        if (!result.Succeeded)
        {
            return HandleServiceError<TaskResponseDto>(result);
        }
        
        return CreatedAtAction(nameof(GetTaskById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTask(int id, UpdateTaskDto dto)
    {
        var result = await _projectTaskService.UpdateTaskAsync(id, dto);

        if (!result.Succeeded)
        {
            return HandleServiceError<bool>(result);
        }
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var result = await _projectTaskService.DeleteTaskAsync(id);
        
        return !result.Succeeded ? HandleServiceError<bool>(result) : NoContent();
    }
}