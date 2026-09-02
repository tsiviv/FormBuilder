namespace FormBuilder.Application.DTOs;

public class FormFieldDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool Required { get; set; }
    public List<string>? Options { get; set; }
}
