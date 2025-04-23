using SATWeb.Models;

namespace SATWeb.ViewModels;

public class AberturaChamadoViewModel
{
    public ChamadoModel Chamado { get; set; }

    public List<UsuarioModel> TodosUsuarios { get; set; }
    public List<DepartamentoModel> TodosDepartamentos { get; set; }
    
    public int UsuarioSelecionadoId { get; set; }
    public int DepartamentoSelecionadoId { get; set; }
}