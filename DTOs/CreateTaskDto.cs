using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs;

public class CreateTaskDto
{
    private string _title = string.Empty;

    [Required(ErrorMessage = "O título é obrigatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve conter entre 3 e 100 caracteres.")]
    public string Title
    {
        get => _title;
        set => _title = value?.Trim() ?? string.Empty;
    }
    
    public string? Description { get; set;  }
}