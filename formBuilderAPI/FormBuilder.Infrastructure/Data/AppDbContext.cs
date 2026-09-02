using FormBuilder.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FormTemplate> FormTemplates => Set<FormTemplate>();
    public DbSet<FormField> FormFields => Set<FormField>();
    public DbSet<ApprovalStep> ApprovalSteps => Set<ApprovalStep>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormTemplate>(entity =>
        {
            entity.Property(f => f.Name).IsRequired().HasMaxLength(200);
            entity.Property(f => f.CreatedBy).IsRequired().HasMaxLength(200);

            entity.HasMany(f => f.Fields)
                .WithOne(field => field.FormTemplate)
                .HasForeignKey(field => field.FormTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(f => f.ApprovalSteps)
                .WithOne(step => step.FormTemplate)
                .HasForeignKey(step => step.FormTemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FormField>(entity =>
        {
            entity.Property(f => f.Label).IsRequired().HasMaxLength(200);
            entity.Property(f => f.Type).IsRequired().HasMaxLength(50);
            entity.Property(f => f.OptionsJson).HasMaxLength(2000);
        });

        modelBuilder.Entity<ApprovalStep>(entity =>
        {
            entity.Property(a => a.Name).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Approver).IsRequired().HasMaxLength(200);
            entity.Property(a => a.ActionType).IsRequired().HasMaxLength(50);
        });
    }
}
