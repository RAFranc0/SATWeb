using Microsoft.EntityFrameworkCore;
using SATWeb.Models;

namespace SATWeb.Data;

public class SatWebDbContext : DbContext
{
    public SatWebDbContext(DbContextOptions<SatWebDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=sat.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ChamadoModel>()
            .Property(c => c.Estado)
            .HasConversion<string>();
    }

    public DbSet<DepartamentoModel> Departamentos { get; set; }
    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<ChamadoModel> Chamados { get; set; }
}