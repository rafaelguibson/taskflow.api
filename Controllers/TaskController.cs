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
    public IActionResult GetAll()
    {
        return Ok(Tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        foreach (var taskItem in Tasks)
        {
            if (taskItem.Id == id)
            {
                return Ok(taskItem);
            }
        }

        return NotFound();
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
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
        TaskItem? taskToDelete = null;

        foreach (var taskItem in Tasks)
        {
            if (taskItem.Id == id)
            {
                taskToDelete = taskItem;
            }
        }

        if (taskToDelete == null)
            return NotFound();

        Tasks.Remove(taskToDelete);
        return Ok(taskToDelete);
    }
}