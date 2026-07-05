using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Project>  Projects { get; set; }
    public DbSet<ProjectTask>  ProjectTasks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Description)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Description)
                .HasMaxLength(1000);
            
            entity.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(30);
            
            entity.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();
            
            entity.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(30);
        });
    }
    
}