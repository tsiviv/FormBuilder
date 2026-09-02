namespace FormBuilder.Infrastructure.Entities;

public class FormField
{
    public int Id { get; set; }

    public int FormTemplateId { get; set; }
    public FormTemplate FormTemplate { get; set; } = null!;

    public required string Label { get; set; }
    public required string Type { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }
}
