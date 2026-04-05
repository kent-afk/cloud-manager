using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;
using Microsoft.EntityFrameworkCore;
namespace Identity.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<RegisteredUser> Users { get; init; } 

    protected override void 
        OnModelCreating(ModelBuilder modelBuilder) // modelbuilder for mapping(сопоставления) on model creating c# classes become database tables
    {
        modelBuilder.Entity<RegisteredUser>(entity => //entity is API to construct 
        {
            entity.HasKey(u => u.UserId); //primary key

            entity.Property(u => u.Email)
                .HasConversion(v => v.Value, // Save value object -> string (in db)
                    v => Email.Create(v) // Load from db -> value object
                ).IsRequired();

            entity.HasIndex(u => u.Email).IsUnique(); 
        });
    }
        
}