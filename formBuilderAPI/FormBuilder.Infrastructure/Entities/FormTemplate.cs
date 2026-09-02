namespace FormBuilder.Infrastructure.Entities;

public class FormTemplate
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }

    public ICollection<FormField> Fields { get; set; } = new List<FormField>();
    public ICollection<ApprovalStep> ApprovalSteps { get; set; } = new List<ApprovalStep>();
}
