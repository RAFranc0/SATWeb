using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATWeb.Data;
using SATWeb.Models;

namespace SATWeb.Controllers;

public class DepartamentosController : Controller
{
    private readonly SatWebDbContext _satdb;
    
    public DepartamentosController(SatWebDbContext satdb)
    {
        _satdb = satdb;
    }
    
    [HttpGet]
    public async Task<IActionResult> ObterDepartamento(int id)
    {
        var departamento = await _satdb.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        return Json(departamento);
    }

    [HttpGet]
    public IActionResult CadastrarDepartamento()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditarDepartamento(int? id = null, bool error = false)
    {
        var listaDeDepartamentos = await _satdb.Departamentos.ToListAsync();
        DepartamentoModel departamentoSelecionado = null;
        
        if (id.HasValue)
        {
            departamentoSelecionado = await _satdb.Departamentos.FindAsync(id);
        }
        
        ViewBag.Departamentos = listaDeDepartamentos;
        ViewBag.DepartamentoSelecionado = departamentoSelecionado;
        ViewBag.Erro = error;
        
        return View("EditarDepartamento");
    }
    
    [HttpGet]
    public async Task<IActionResult> ExcluirDepartamento(int? id = null, bool error = false)
    {
        var listaDeDepartamentos = await _satdb.Departamentos.ToListAsync();
        DepartamentoModel departamentoSelecionado = null;

        if (id.HasValue)
        {
            departamentoSelecionado = await _satdb.Departamentos.FindAsync(id);
        }

        ViewBag.Departamentos = listaDeDepartamentos;
        ViewBag.DepartamentoSelecionado = departamentoSelecionado;
        ViewBag.Erro = error;

        return View("ExcluirDepartamento");
    }

    
    [HttpGet]
    public async Task<IActionResult> ListarDepartamentos()
    {
        var listaDeDepartamentos = await _satdb.Departamentos.ToListAsync();
        return View(listaDeDepartamentos);
    }
    
    [HttpPost]
    public async Task<IActionResult> Cadastrar(DepartamentoModel departamento)
    {
        if (!ModelState.IsValid)
        {
            return View("CadastrarDepartamento", departamento); 
        }

        try
        {
            _satdb.Departamentos.Add(departamento);
            await _satdb.SaveChangesAsync();
            return RedirectToAction("ListarDepartamentos");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Não foi possível salvar. Tente novamente.");
            return View("CadastrarDepartamento", departamento);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        var departamento = await _satdb.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }

        try
        {
            _satdb.Departamentos.Remove(departamento);
            await _satdb.SaveChangesAsync();
            return RedirectToAction("ListarDepartamentos");
        }
        catch (DbUpdateException ex)
        {
            return RedirectToAction("ExcluirDepartamento", new { id, error = true });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, DepartamentoModel departamentoAtualizado)
    {
        if (id != departamentoAtualizado.Id)
            return BadRequest();


        if (!ModelState.IsValid)
            return View("EditarDepartamento", departamentoAtualizado);
        
        var departamentoExistente = await _satdb.Departamentos.FindAsync(id);
        if (departamentoExistente == null)
            return NotFound();

        try
        {
            departamentoExistente.Nome = departamentoAtualizado.Nome;
            await _satdb.SaveChangesAsync();
            TempData["Mensagem"] = "Departamento atualizado com sucesso!";
            return RedirectToAction("ListarDepartamentos", "Departamentos");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Erro ao atualizar o departamento.");
            return View("EditarDepartamento", departamentoAtualizado);
        }
    }
}