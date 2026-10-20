using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Services;

public interface ITaskService
{
    IEnumerable<TaskResponseDto> GetAll(bool? completed, string? title);
    TaskResponseDto? GetById(int id);
    TaskResponseDto Create(CreateTaskDto dto);
    TaskResponseDto? Update(int id, UpdateTaskDto dto);
    bool Delete(int id);
}