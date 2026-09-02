using FormBuilder.Application.DTOs;

namespace FormBuilder.Application.Interfaces;

public interface IFormTemplateRepository
{
    Task<FormTemplateDto> CreateAsync(CreateFormTemplateDto dto, DateTime createdAt);
    Task<List<FormTemplateDto>> GetAllAsync();
    Task<FormTemplateDto?> GetByIdAsync(int id);
}
