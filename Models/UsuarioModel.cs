namespace SATWeb.Models;

public class UsuarioModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int DepartamentoId { get; set; }
    public DepartamentoModel Departamento { get; set; }
}