using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Application.DTOs;

public class CreateFormFieldDto : IValidatableObject
{
    [Required, MaxLength(200)]
    public string Label { get; set; } = string.Empty;

    [Required, RegularExpression(FieldTypes.Pattern, ErrorMessage = FieldTypes.ErrorMessage)]
    public string Type { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Order must be a positive integer")]
    public int Order { get; set; }

    public bool Required { get; set; }

    public List<string>? Options { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FieldTypes.RequiresOptions(Type))
        {
            var nonEmptyOptions = Options?.Where(o => !string.IsNullOrWhiteSpace(o)).ToList() ?? new List<string>();
            if (nonEmptyOptions.Count == 0)
            {
                yield return new ValidationResult(
                    "At least one option is required for select and radio fields",
                    new[] { nameof(Options) });
            }
        }
    }
}
