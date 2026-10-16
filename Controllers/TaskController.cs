using Microsoft.AspNetCore.Mvc;
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
        
        return Ok(filteredTasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title) || string.IsNullOrEmpty(task.Title))
            return BadRequest("O título não pode estar vázio ou só conter espaços em branco");
        
        task.Id = NextId++;
        
        Tasks.Add(task);

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem task)
    {
        foreach (var taskItem in Tasks )
        {
            if (taskItem.Id == id)
            {
                taskItem.Title = task.Title;
                taskItem.Description = task.Description;
                taskItem.Completed = task.Completed;
                return Ok(task);
            }
        }
        return NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        TaskItem? taskToDelete = Tasks.FirstOrDefault(t => t.Id == id);

        if (taskToDelete == null)
            return NotFound();

        Tasks.Remove(taskToDelete);
        NextId--;
        return Ok(taskToDelete);
    }
    
}