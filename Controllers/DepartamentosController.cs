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
    public IActionResult CadastrarDepartamento()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EditarDepartamento()
    {
        return View("EditarDepartamento");
    }

    [HttpGet]
    public IActionResult ExcluirDepartamento()
    {
        return View("ExcluirDepartamento");
    }
    
    [HttpGet("listar")]
    public async Task<IActionResult> ListarDepartamentos()
    {
        var listaDeDepartamentos = _satdb.Departamentos.ToListAsync();
        return View(listaDeDepartamentos);
    }
    
    [HttpPost("cadastrar")]
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
    
    [HttpPost("excluir")]
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
    
    [HttpGet("editar/{id}")]
    public async Task<IActionResult> Editar(int id)
    {
        var departamento = await _satdb.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        return View("EditarDepartamento", departamento);
    }

    [HttpPost("editar/{id}")]
    public async Task<IActionResult> Editar(int id, DepartamentoModel departamentoAtualizado)
    {
        if (id != departamentoAtualizado.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("EditarDepartamento", departamentoAtualizado);
        }

        try
        {
            _satdb.Update(departamentoAtualizado);
            await _satdb.SaveChangesAsync();
            return RedirectToAction("ListarDepartamentos");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Erro ao atualizar.");
            return View("EditarDepartamento", departamentoAtualizado);
        }
    }
}