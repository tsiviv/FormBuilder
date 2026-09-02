using FormBuilder.Application.DTOs;

namespace FormBuilder.Application.Interfaces;

public interface IFormTemplateService
{
    Task<FormTemplateDto> CreateFormAsync(CreateFormTemplateDto dto);
    Task<List<FormTemplateDto>> GetFormsAsync();
    Task<FormTemplateDto?> GetFormByIdAsync(int id);
}
