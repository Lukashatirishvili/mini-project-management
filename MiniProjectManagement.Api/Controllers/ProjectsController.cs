using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.DTOs.Projects;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : Controller
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
        var project = await _projectService.GetProjectByIdAsync(id);
        
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> CreateProject(CreateProjectDto dto)
    {
        var project = await _projectService.CreateProjectAsync(dto);
        
        return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProject(int id, UpdateProjectDto dto)
    {
        var updated = await _projectService.UpdateProjectAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProject(int id)
    {
        var project = await _projectService.DeleteProjectAsync(id);

        if (!project)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}