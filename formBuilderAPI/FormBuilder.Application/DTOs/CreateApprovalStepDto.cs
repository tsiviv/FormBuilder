using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Application.DTOs;

public class CreateApprovalStepDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Order must be a positive integer")]
    public int Order { get; set; }

    [Required, MaxLength(200)]
    public string Approver { get; set; } = string.Empty;

    [Required, RegularExpression("^(Approve|Reject|ApproveReject)$", ErrorMessage = "ActionType must be one of: Approve, Reject, ApproveReject")]
    public string ActionType { get; set; } = string.Empty;
}
