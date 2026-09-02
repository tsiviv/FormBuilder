using FormBuilder.Application.Interfaces;
using FormBuilder.Application.Services;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFormTemplateRepository, FormTemplateRepository>();
builder.Services.AddScoped<IFormTemplateService, FormTemplateService>();

const string AngularDevCorsPolicy = "AngularDevCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4299")
              .WithMethods("GET", "POST")
              .AllowAnyHeader());
});

var app = builder.Build();

// Global exception handler: converts any unhandled exception into a generic
// ProblemDetails 500 response, never leaking stack traces or internal details,
// regardless of ASPNETCORE_ENVIRONMENT. Expected exceptions (e.g. validation)
// are still handled where they occur and never reach this point.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AngularDevCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
