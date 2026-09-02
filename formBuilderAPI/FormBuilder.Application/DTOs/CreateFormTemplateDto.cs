using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Application.DTOs;

public class CreateFormTemplateDto : IValidatableObject
{
    public const int MaxFields = 100;
    public const int MaxApprovalSteps = 100;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "At least one field is required")]
    [MaxLength(MaxFields, ErrorMessage = "A form cannot have more than 100 fields")]
    public List<CreateFormFieldDto> Fields { get; set; } = new();

    [Required]
    [MinLength(1, ErrorMessage = "At least one approval step is required")]
    [MaxLength(MaxApprovalSteps, ErrorMessage = "A form cannot have more than 100 approval steps")]
    public List<CreateApprovalStepDto> ApprovalSteps { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Fields is { Count: > 0 } && !HasSequentialOrder(Fields.Select(f => f.Order)))
        {
            yield return new ValidationResult(
                "Field order values must be sequential starting at 1 with no duplicates or gaps.",
                new[] { nameof(Fields) });
        }

        if (ApprovalSteps is { Count: > 0 } && !HasSequentialOrder(ApprovalSteps.Select(s => s.Order)))
        {
            yield return new ValidationResult(
                "Approval step order values must be sequential starting at 1 with no duplicates or gaps.",
                new[] { nameof(ApprovalSteps) });
        }
    }

    private static bool HasSequentialOrder(IEnumerable<int> orders)
    {
        var sorted = orders.OrderBy(o => o).ToList();
        return sorted.SequenceEqual(Enumerable.Range(1, sorted.Count));
    }
}
