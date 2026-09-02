namespace FormBuilder.Infrastructure.Entities;

public class ApprovalStep
{
    public int Id { get; set; }

    public int FormTemplateId { get; set; }
    public FormTemplate FormTemplate { get; set; } = null!;

    public required string Name { get; set; }
    public int Order { get; set; }
    public required string Approver { get; set; }
    public required string ActionType { get; set; }
}
