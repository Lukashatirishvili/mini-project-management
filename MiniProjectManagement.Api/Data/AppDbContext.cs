using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Project>  Projects { get; set; }
}