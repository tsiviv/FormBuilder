using System.Text.Json;
using FormBuilder.Application.DTOs;
using FormBuilder.Application.Interfaces;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Infrastructure.Repositories;

public class FormTemplateRepository : IFormTemplateRepository
{
    private readonly AppDbContext _context;

    public FormTemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FormTemplateDto> CreateAsync(CreateFormTemplateDto dto, DateTime createdAt)
    {
        var formTemplate = new FormTemplate
        {
            Name = dto.Name,
            CreatedBy = dto.CreatedBy,
            CreatedAt = createdAt,
            Fields = dto.Fields.Select(f => new FormField
            {
                Label = f.Label,
                Type = f.Type,
                Order = f.Order,
                Required = f.Required,
                OptionsJson = SerializeOptions(f.Options)
            }).ToList(),
            ApprovalSteps = dto.ApprovalSteps.Select(s => new ApprovalStep
            {
                Name = s.Name,
                Order = s.Order,
                Approver = s.Approver,
                ActionType = s.ActionType
            }).ToList()
        };

        _context.FormTemplates.Add(formTemplate);
        await _context.SaveChangesAsync();

        return ToDto(formTemplate);
    }

    public async Task<List<FormTemplateDto>> GetAllAsync()
    {
        var formTemplates = await _context.FormTemplates
            .AsNoTracking()
            .Include(f => f.Fields)
            .Include(f => f.ApprovalSteps)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        return formTemplates.Select(ToDto).ToList();
    }

    public async Task<FormTemplateDto?> GetByIdAsync(int id)
    {
        var formTemplate = await _context.FormTemplates
            .AsNoTracking()
            .Include(f => f.Fields)
            .Include(f => f.ApprovalSteps)
            .FirstOrDefaultAsync(f => f.Id == id);

        return formTemplate is null ? null : ToDto(formTemplate);
    }

    private static FormTemplateDto ToDto(FormTemplate formTemplate) => new()
    {
        Id = formTemplate.Id,
        Name = formTemplate.Name,
        CreatedAt = formTemplate.CreatedAt,
        CreatedBy = formTemplate.CreatedBy,
        Fields = formTemplate.Fields
            .OrderBy(f => f.Order)
            .Select(f => new FormFieldDto
            {
                Id = f.Id,
                Label = f.Label,
                Type = f.Type,
                Order = f.Order,
                Required = f.Required,
                Options = DeserializeOptions(f.OptionsJson)
            }).ToList(),
        ApprovalSteps = formTemplate.ApprovalSteps
            .OrderBy(s => s.Order)
            .Select(s => new ApprovalStepDto
            {
                Id = s.Id,
                Name = s.Name,
                Order = s.Order,
                Approver = s.Approver,
                ActionType = s.ActionType
            }).ToList()
    };

    private static string? SerializeOptions(List<string>? options)
    {
        var cleaned = options?.Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
        return cleaned is { Count: > 0 } ? JsonSerializer.Serialize(cleaned) : null;
    }

    private static List<string>? DeserializeOptions(string? optionsJson) =>
        string.IsNullOrEmpty(optionsJson) ? null : JsonSerializer.Deserialize<List<string>>(optionsJson);
}
