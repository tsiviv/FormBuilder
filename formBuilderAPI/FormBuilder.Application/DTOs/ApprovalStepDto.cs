namespace FormBuilder.Application.DTOs;

public class ApprovalStepDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Approver { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
}
