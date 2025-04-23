using SATWeb.Models;

namespace SATWeb.ViewModels;

public class CadastroUsuarioViewModel
{
    public UsuarioModel Usuario { get; set; }
    public List<DepartamentoModel> ListaDepartamentos { get; set; }
}