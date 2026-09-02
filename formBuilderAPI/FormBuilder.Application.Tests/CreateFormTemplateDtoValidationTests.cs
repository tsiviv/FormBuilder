using System.ComponentModel.DataAnnotations;
using FormBuilder.Application.DTOs;
using Xunit;

namespace FormBuilder.Application.Tests;

public class CreateFormTemplateDtoValidationTests
{
    private static List<ValidationResult> Validate(CreateFormTemplateDto dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    private static CreateFormTemplateDto ValidBase() => new()
    {
        Name = "Test Form",
        CreatedBy = "tester",
        Fields = new List<CreateFormFieldDto>
        {
            new() { Label = "F1", Type = "text", Order = 1 }
        },
        ApprovalSteps = new List<CreateApprovalStepDto>
        {
            new() { Name = "S1", Order = 1, Approver = "m", ActionType = "Approve" }
        }
    };

    [Fact]
    public void ValidSequentialOrders_IsValid()
    {
        var results = Validate(ValidBase());

        Assert.Empty(results);
    }

    [Fact]
    public void Fields_ExceedingMaximum_IsInvalid()
    {
        var dto = ValidBase();
        dto.Fields = Enumerable.Range(1, CreateFormTemplateDto.MaxFields + 1)
            .Select(i => new CreateFormFieldDto { Label = $"F{i}", Type = "text", Order = i })
            .ToList();

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateFormTemplateDto.Fields)));
    }

    [Fact]
    public void ApprovalSteps_ExceedingMaximum_IsInvalid()
    {
        var dto = ValidBase();
        dto.ApprovalSteps = Enumerable.Range(1, CreateFormTemplateDto.MaxApprovalSteps + 1)
            .Select(i => new CreateApprovalStepDto { Name = $"S{i}", Order = i, Approver = "m", ActionType = "Approve" })
            .ToList();

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateFormTemplateDto.ApprovalSteps)));
    }

    [Fact]
    public void DuplicateFieldOrder_IsInvalid()
    {
        var dto = ValidBase();
        dto.Fields = new List<CreateFormFieldDto>
        {
            new() { Label = "F1", Type = "text", Order = 1 },
            new() { Label = "F2", Type = "date", Order = 1 }
        };

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateFormTemplateDto.Fields)));
    }

    [Fact]
    public void GappedFieldOrder_IsInvalid()
    {
        var dto = ValidBase();
        dto.Fields = new List<CreateFormFieldDto>
        {
            new() { Label = "F1", Type = "text", Order = 1 },
            new() { Label = "F2", Type = "date", Order = 3 }
        };

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateFormTemplateDto.Fields)));
    }

    [Fact]
    public void DuplicateApprovalStepOrder_IsInvalid()
    {
        var dto = ValidBase();
        dto.ApprovalSteps = new List<CreateApprovalStepDto>
        {
            new() { Name = "S1", Order = 1, Approver = "m1", ActionType = "Approve" },
            new() { Name = "S2", Order = 1, Approver = "m2", ActionType = "Reject" }
        };

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateFormTemplateDto.ApprovalSteps)));
    }

    [Fact]
    public void GappedApprovalStepOrder_IsInvalid()
    {
        var dto = ValidBase();
        dto.ApprovalSteps = new List<CreateApprovalStepDto>
        {
            new() { Name = "S1", Order = 1, Approver = "m1", ActionType = "Approve" },
            new() { Name = "S2", Order = 5, Approver = "m2", ActionType = "Reject" }
        };

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateFormTemplateDto.ApprovalSteps)));
    }
}
