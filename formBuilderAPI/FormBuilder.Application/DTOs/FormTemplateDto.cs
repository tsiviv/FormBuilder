namespace FormBuilder.Application.DTOs;

public class FormTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public List<FormFieldDto> Fields { get; set; } = new();
    public List<ApprovalStepDto> ApprovalSteps { get; set; } = new();
}
