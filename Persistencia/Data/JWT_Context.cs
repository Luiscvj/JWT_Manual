using System.Reflection;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;

public class JWT_Context : DbContext
{
    public JWT_Context(DbContextOptions<JWT_Context> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder modelBuilder)
    {
        modelBuilder.Properties<string>().HaveMaxLength(150);
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pais> Paises { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}