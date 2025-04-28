using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATWeb.Data;
using SATWeb.Enums;
using SATWeb.Models;
using SATWeb.ViewModels;

namespace SATWeb.Controllers;

public class ChamadosController(SatWebDbContext satdb) : Controller
{
    //===============================================
    //REGIÃO: Actions de Abertura de Chamados
    //===============================================

    [HttpGet]
    public async Task<IActionResult> NovoChamado()
    {
        var viewModel = new AberturaChamadoViewModel
        {
            TodosDepartamentos = await satdb.Departamentos.ToListAsync(),
            TodosUsuarios = await satdb.Usuarios.ToListAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Abrir(AberturaChamadoViewModel aberturaChamado)
    {
        try
        {
            var usuarioSelecionado = await satdb.Usuarios
                .Include(u => u.Departamento)
                .FirstOrDefaultAsync(u => u.Id == aberturaChamado.UsuarioSelecionadoId);

            if (usuarioSelecionado == null)
            {
                ModelState.AddModelError("UsuarioSelecionadoId", "Usuário não encontrado");
                aberturaChamado.TodosDepartamentos = await satdb.Departamentos.ToListAsync();
                aberturaChamado.TodosUsuarios = await satdb.Usuarios.ToListAsync();
                return View("NovoChamado", aberturaChamado);
            }

            var novoChamado = new ChamadoModel
            {
                Data = DateTime.Now,
                Estado = EstadoChamado.Aberto,
                UsuarioId = aberturaChamado.UsuarioSelecionadoId,
                DepartamentoId = usuarioSelecionado.DepartamentoId,
                Descricao = FormatarDescricaoChamado(aberturaChamado.Chamado.Descricao)
            };

            satdb.Chamados.Add(novoChamado);
            await satdb.SaveChangesAsync();

            return RedirectToAction("ListarChamados");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

            ModelState.AddModelError("", "Não foi possível abrir o chamado. Verifique os dados.");
            aberturaChamado.TodosDepartamentos = await satdb.Departamentos.ToListAsync();
            aberturaChamado.TodosUsuarios = await satdb.Usuarios.ToListAsync();
            return View("NovoChamado", aberturaChamado);
        }
    }

    private string FormatarDescricaoChamado(string descricao)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{EstadoChamado.Aberto} em {DateTime.Now:dd/MM/yyyy HH:mm}")
            .AppendLine("*** Descrição ***")
            .AppendLine(descricao);

        return sb.ToString();
    }

    //===============================================
    //REGIÃO: Actions de Listagem de Chamados
    //===============================================

    [HttpGet]
    public async Task<IActionResult> ListarChamados()
    {
        var viewModel = new AlterarChamadoModel
        {
            TodosChamados = await satdb.Chamados
                .OrderByDescending(c => c.Data)
                .ToListAsync(),
        };
        return View(viewModel);
    }

    //============================================================
    //REGIÃO: Actions de Edição de Chamados (reabrir e encerrar)
    //============================================================

    [HttpPost]
    public async Task<IActionResult> Reabrir(int id, AlterarChamadoModel reabertura)
    {
        var chamado = await satdb.Chamados.FindAsync(id);

        if (chamado == null)
            return NotFound();

        if (!Enum.IsDefined(typeof(EstadoChamado), reabertura.Estado))
        {
            ModelState.AddModelError("", "Estado inválido. Utilize apenas 'Aberto', 'Reaberto' ou 'Encerrado'");
            return View("ListarChamados", reabertura);
        }

        reabertura.Estado = EstadoChamado.Reaberto;
        chamado.Estado = reabertura.Estado;

        var sb = new StringBuilder(chamado.Descricao);
        sb.AppendLine()
            .AppendLine($"{reabertura.Estado} em {DateTime.UtcNow:dd/MM/yyyy HH:mm}")
            .AppendLine("*** Atualização ***")
            .AppendLine(reabertura.Descricao);

        chamado.Descricao = sb.ToString();

        await satdb.SaveChangesAsync();
        return RedirectToAction("ListarChamados");
    }

    [HttpPost]
    public async Task<IActionResult> Encerrar(int id, AlterarChamadoModel encerramento)
    {
        var chamado = await satdb.Chamados.FindAsync(id);

        if (chamado == null)
            return NotFound();

        if (!Enum.IsDefined(typeof(EstadoChamado), encerramento.Estado))
        {
            ModelState.AddModelError("", "Estado inválido.");
            return View("ListarChamados", encerramento);
        }

        encerramento.Estado = EstadoChamado.Encerrado;
        chamado.Estado = encerramento.Estado;

        var sb = new StringBuilder(chamado.Descricao);
        sb.AppendLine()
            .AppendLine($"{encerramento.Estado} em {DateTime.UtcNow:dd/MM/yyyy HH:mm}")
            .AppendLine("*** Motivo ***")
            .AppendLine(encerramento.Descricao);

        chamado.Descricao = sb.ToString();

        await satdb.SaveChangesAsync();
        return RedirectToAction("ListarChamados");
    }
}