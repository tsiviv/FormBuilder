using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Application.DTOs;

public class CreateFormFieldDto
{
    [Required, MaxLength(200)]
    public string Label { get; set; } = string.Empty;

    [Required, RegularExpression("^(text|date)$", ErrorMessage = "Type must be one of: text, date")]
    public string Type { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Order must be a positive integer")]
    public int Order { get; set; }

    public bool Required { get; set; }
}
