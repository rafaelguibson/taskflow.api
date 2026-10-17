using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Models;

public class TaskItem
{
    private string _title = string.Empty;
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve conter de 3 a 10 caracteres.")]
    public string Title
    {
        get => _title;
        set => _title = value?.Trim() ?? string.Empty;
    }
    public string? Description { get; set; }
    public bool Completed { get; set; }
    
}