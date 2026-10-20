using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

public class TaskService: ITaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IEnumerable<TaskResponseDto> GetAll(bool? completed, string? title)
    {
        IEnumerable<TaskItem> filteredTasks = _tasks;

        if (completed.HasValue)
            filteredTasks = _tasks.Where(t => t.Completed == completed.Value);

        if (!string.IsNullOrWhiteSpace(title))
            filteredTasks = _tasks.Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        return filteredTasks.Select(t => new TaskResponseDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Completed = t.Completed
        });

    }

    public TaskResponseDto? GetById(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return null;

        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed
        };
    }

    public TaskResponseDto Create(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Id = _nextId++,
            Title = dto.Title,
            Description = dto.Description,
            Completed = false
        };
        
        _tasks.Add(task);
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed
        };
    }

    public TaskResponseDto? Update(int id, UpdateTaskDto dto)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return null;

        task.Id = id;
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Completed = dto.Completed;

        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed
        };
    }

    public bool Delete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        
        if (task == null) return false;

        _tasks.Remove(task);
        return true;
    }
}