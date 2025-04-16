using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATWeb.Data;
using SATWeb.Models;

namespace SATWeb.Controllers;

public class DepartamentosController(SatWebDbContext satdb) : Controller
{
    //============================================
    //REGIÃO: Actions de API/JSON
    //============================================
    
    [HttpGet]
    public async Task<IActionResult> ObterDepartamento(int id)
    {
        var departamento = await satdb.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        return Json(departamento);
    }
    
    //===============================================
    //REGIÃO: Actions de Cadastro de Departamentos
    //===============================================

    [HttpGet]
    public IActionResult CadastrarDepartamento()
    {
        return View();
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
            satdb.Departamentos.Add(departamento);
            await satdb.SaveChangesAsync();
            return RedirectToAction("ListarDepartamentos");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Não foi possível salvar. Tente novamente.");
            return View("CadastrarDepartamento", departamento);
        }
    }
    
    //===============================================
    //REGIÃO: Actions de Listagem de Departamentos
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> ListarDepartamentos()
    {
        var listaDeDepartamentos = await satdb.Departamentos.ToListAsync();
        return View(listaDeDepartamentos);
    }
    
    //===============================================
    //REGIÃO: Actions de Edição de Departamentos
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> EditarDepartamento(int? id = null, bool error = false)
    {
        var listaDeDepartamentos = await satdb.Departamentos.ToListAsync();
        DepartamentoModel? departamentoSelecionado = null;
        
        if (id.HasValue)
        {
            departamentoSelecionado = await satdb.Departamentos.FindAsync(id);
        }
        
        ViewBag.Departamentos = listaDeDepartamentos;
        ViewBag.DepartamentoSelecionado = departamentoSelecionado;
        ViewBag.Erro = error;
        
        return View("EditarDepartamento");
    }
    
    [HttpPost]
    public async Task<IActionResult> Editar(int id, DepartamentoModel departamentoAtualizado)
    {
        if (id != departamentoAtualizado.Id)
            return BadRequest();


        if (!ModelState.IsValid)
            return View("EditarDepartamento", departamentoAtualizado);
        
        var departamentoExistente = await satdb.Departamentos.FindAsync(id);
        if (departamentoExistente == null)
            return NotFound();

        try
        {
            departamentoExistente.Nome = departamentoAtualizado.Nome;
            await satdb.SaveChangesAsync();
            TempData["Mensagem"] = "Departamento atualizado com sucesso!";
            return RedirectToAction("ListarDepartamentos", "Departamentos");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Erro ao atualizar o departamento.");
            return View("EditarDepartamento", departamentoAtualizado);
        }
    }
    
    //===============================================
    //REGIÃO: Actions de Exclusão de Departamentos
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> ExcluirDepartamento(int? id = null, bool error = false)
    {
        var listaDeDepartamentos = await satdb.Departamentos.ToListAsync();
        DepartamentoModel? departamentoSelecionado = null;

        if (id.HasValue)
        {
            departamentoSelecionado = await satdb.Departamentos.FindAsync(id);
        }

        ViewBag.Departamentos = listaDeDepartamentos;
        ViewBag.DepartamentoSelecionado = departamentoSelecionado;
        ViewBag.Erro = error;

        return View("ExcluirDepartamento");
    }

    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        var departamento = await satdb.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }

        try
        {
            satdb.Departamentos.Remove(departamento);
            await satdb.SaveChangesAsync();
            return RedirectToAction("ListarDepartamentos");
        }
        catch (DbUpdateException)
        {
            return RedirectToAction("ExcluirDepartamento", new { id, error = true });
        }
    }
}