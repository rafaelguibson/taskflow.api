using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    public static readonly List<TaskItem> Tasks = new();

    private static int NextId = 1;

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] bool? completed = null, 
        [FromQuery] string? title = null
        )
    {
        IEnumerable<TaskItem> filteredTasks = Tasks;

        if (completed.HasValue)
        {
            filteredTasks = filteredTasks
                .Where(t => t.Completed == completed.Value);
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            filteredTasks = filteredTasks
                .Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        var response = filteredTasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Completed = t.Completed
            });
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        var response = new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed
        };
        
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create(CreateTaskDto dto)
    {
        var task = new TaskItem()
        {
            Id = NextId++,
            Title = dto.Title,
            Description = dto.Description,
            Completed = false
        };
        Tasks.Add(task);
        var response = new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed
        };
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateTaskDto dto)
    {
        var taskItem = Tasks.FirstOrDefault(t => t.Id == id);

        if (taskItem == null)
            return NotFound();

        taskItem.Title = dto.Title;
        taskItem.Description = dto.Description;
        taskItem.Completed = dto.Completed;

        var response = new TaskResponseDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            Completed = taskItem.Completed
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        TaskItem? taskToDelete = Tasks.FirstOrDefault(t => t.Id == id);

        if (taskToDelete == null)
            return NotFound();

        Tasks.Remove(taskToDelete);
        
        var response = new TaskResponseDto
        {
            Id = taskToDelete.Id,
            Title = taskToDelete.Title,
            Description = taskToDelete.Description,
            Completed = taskToDelete.Completed
        };
        
        return Ok(response);
    }
    
}