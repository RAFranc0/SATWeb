namespace SATWeb.Models;

public class EditarUsuarioViewModel
{
    public UsuarioModel Usuario { get; set; }
    public List<DepartamentoModel> ListaDepartamentos { get; set; }
    public List<UsuarioModel> TodosUsuarios { get; set; }
}