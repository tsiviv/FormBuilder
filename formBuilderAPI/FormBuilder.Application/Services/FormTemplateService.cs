using System.ComponentModel.DataAnnotations;
using FormBuilder.Application.DTOs;
using FormBuilder.Application.Exceptions;
using FormBuilder.Application.Interfaces;

namespace FormBuilder.Application.Services;

public class FormTemplateService : IFormTemplateService
{
    private readonly IFormTemplateRepository _repository;

    public FormTemplateService(IFormTemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<FormTemplateDto> CreateFormAsync(CreateFormTemplateDto dto)
    {
        var errors = ValidateCreateFormTemplate(dto);
        if (errors.Count > 0)
        {
            throw new FormValidationException(errors);
        }

        var createdAt = DateTime.UtcNow;
        return await _repository.CreateAsync(dto, createdAt);
    }

    public Task<List<FormTemplateDto>> GetFormsAsync() => _repository.GetAllAsync();

    public Task<FormTemplateDto?> GetFormByIdAsync(int id) => _repository.GetByIdAsync(id);

    private static List<string> ValidateCreateFormTemplate(CreateFormTemplateDto dto)
    {
        var errors = new List<string>();
        ValidateDataAnnotations(dto, errors);

        foreach (var field in dto.Fields)
        {
            ValidateDataAnnotations(field, errors);
        }

        foreach (var step in dto.ApprovalSteps)
        {
            ValidateDataAnnotations(step, errors);
        }

        return errors;
    }

    private static void ValidateDataAnnotations(object instance, List<string> errors)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
        errors.AddRange(results.Select(r => r.ErrorMessage ?? "Invalid value"));
    }
}
