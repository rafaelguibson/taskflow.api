using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService service) : ControllerBase
{
    private readonly ITaskService _taskService = service;

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] bool? completed = null, 
        [FromQuery] string? title = null
        )
    {
        var response = _taskService.GetAll(completed, title);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var response = _taskService.GetById(id);

        if (response == null) return NotFound();
        
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create(CreateTaskDto dto)
    {
        var response = _taskService.Create(dto);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateTaskDto dto)
    {
        var response = _taskService.Update(id, dto);

        if (response == null) return NotFound();

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _taskService.Delete(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
    
}