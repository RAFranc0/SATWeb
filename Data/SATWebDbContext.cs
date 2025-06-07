using Microsoft.EntityFrameworkCore;
using SATWeb.Models;

namespace SATWeb.Data;

public class SatWebDbContext : DbContext
{
    public SatWebDbContext(DbContextOptions<SatWebDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=yamanote.proxy.rlwy.net;Port=50520;Username=postgres;Password=WdVJDXiHvlxDptnxyMAyVOpQHvwxaPTe;Database=railway;Pooling=true;SSL Mode=Require;Trust Server Certificate=true;");

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