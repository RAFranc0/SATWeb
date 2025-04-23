using SATWeb.Data;
using SATWeb.Enums;

namespace SATWeb.Models;

public class AlterarChamadoModel()
{
    public List<ChamadoModel> TodosChamados { get; set; }
    public EstadoChamado Estado { get; set; }
    public string Descricao { get; set; } = string.Empty;
}