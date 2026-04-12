using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<RegisteredUser> Users => Set<RegisteredUser>(); // to avoid static warning of null 
    
    //TPH
    protected override void 
        OnModelCreating(ModelBuilder modelBuilder) // builder for mapping(сопоставления) on model creating c# classes become database tables
    {
        modelBuilder.Entity<RegisteredUser>(entity => //entity is API to construct 
        {
            entity.HasKey(u => u.UserId); //primary key

            entity.Property(u => u.UserId)
                .HasConversion(
                v => v.ToString(),
                v => Guid.Parse(v)
                ).HasColumnName("UserId")
                .ValueGeneratedOnAdd(); // Gen id when add
            
            entity.Property(u => u.Email)
                .HasConversion(
                v => v.Value, // Save value object -> string (in db)
                v => Email.Create(v) // Load from db -> value object
                ).HasColumnName("Email")
                .IsRequired();

            entity.Property(u => u.HashPassword) // MAIL | PASS
                .HasConversion(
                v => v.Value,
                v => HashPassword.Create(v)
                ).HasColumnName("HashPassword")
                .IsRequired();
            
            entity.HasIndex(u => u.Email).IsUnique(); 
        });
    }
        
}