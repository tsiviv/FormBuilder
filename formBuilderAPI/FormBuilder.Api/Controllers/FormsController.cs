using FormBuilder.Application.DTOs;
using FormBuilder.Application.Exceptions;
using FormBuilder.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.Api.Controllers;

[ApiController]
[Route("api/forms")]
public class FormsController : ControllerBase
{
    private readonly IFormTemplateService _formTemplateService;

    public FormsController(IFormTemplateService formTemplateService)
    {
        _formTemplateService = formTemplateService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(FormTemplateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateForm([FromBody] CreateFormTemplateDto dto)
    {
        try
        {
            var created = await _formTemplateService.CreateFormAsync(dto);
            return CreatedAtAction(nameof(GetFormById), new { id = created.Id }, created);
        }
        catch (FormValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<FormTemplateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForms()
    {
        var forms = await _formTemplateService.GetFormsAsync();
        return Ok(forms);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FormTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFormById(int id)
    {
        var form = await _formTemplateService.GetFormByIdAsync(id);
        return form is null ? NotFound() : Ok(form);
    }
}
