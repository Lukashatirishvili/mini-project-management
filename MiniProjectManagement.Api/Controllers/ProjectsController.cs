using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.DTOs.Projects;
using MiniProjectManagement.Api.Helpers;
using MiniProjectManagement.Api.Models;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : BaseApiController
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponseDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        
        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetProjectById(int id)
    {
        var result = await _projectService.GetProjectByIdAsync(id);

        if (!result.Succeeded)
        {
            return HandleServiceError<ProjectResponseDto>(result);
        }
        
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> CreateProject(CreateProjectDto dto)
    {
        var result = await _projectService.CreateProjectAsync(dto);

        if (!result.Succeeded)
        {
            return HandleServiceError<ProjectResponseDto>(result);
        }
        
        return CreatedAtAction(nameof(GetProjectById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProject(int id, UpdateProjectDto dto)
    {
        var result = await _projectService.UpdateProjectAsync(id, dto);

        return !result.Succeeded ? HandleServiceError<bool>(result) : NoContent();
    }

    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        return !result.Succeeded ? HandleServiceError<bool>(result) : NoContent();
    }
}