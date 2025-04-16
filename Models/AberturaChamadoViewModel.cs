namespace SATWeb.Models;

public class AberturaChamadoViewModel
{
    public List<UsuarioModel> ListaUsuarios { get; set; }
    public List<DepartamentoModel> ListaDepartamentos { get; set; }
    
    public int UsuarioSelecionadoId { get; set; }
    public int DepartamentoSelecionadoId { get; set; }
}