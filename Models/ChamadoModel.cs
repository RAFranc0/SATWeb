using SATWeb.Enums;

namespace SATWeb.Models;

public class ChamadoModel
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public EstadoChamado Estado { get; set; }
    public int UsuarioId { get; set; }
    public int DepartamentoId { get; set; }
    public string Descricao { get; set; }
    public UsuarioModel Usuario { get; set; }
    public DepartamentoModel Departamento { get; set; }
}