using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATWeb.Data;
using SATWeb.Models;

namespace SATWeb.Controllers;

public class UsuariosController : Controller
{
    private readonly SatWebDbContext _satdb;

    public UsuariosController(SatWebDbContext satdb)
    {
        _satdb = satdb;
    }
    
    public IActionResult CadastrarUsuario()
    {
        ViewBag.Departamentos = _satdb.Departamentos.ToList();
        return View();
    }
    public IActionResult ExcluirUsuario()
    {
        return View();
    }
    public IActionResult EditarUsuario()
    {
        return View();
    }
    
    [HttpGet("listarusuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        var listaDeUsuarios = await _satdb.Usuarios.ToListAsync();
        return View("ListarUsuarios", listaDeUsuarios);
    }

    [HttpPost("cadastrarusuario")]
    public async Task<IActionResult> Cadastrar(UsuarioModel usuario)
    {
        
        if (!ModelState.IsValid)
        {
            return View("CadastrarUsuario", usuario); 
        }

        try
        {
            _satdb.Usuarios.Add(usuario);
            await _satdb.SaveChangesAsync();
            return RedirectToAction("ListarUsuarios");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Não foi possível salvar. Tente novamente.");
            return View("CadastrarUsuario", usuario);
        }
    }

    [HttpPost("excluirusuario")]
    public async Task<IActionResult> Excluir(int id)
    {
        var usuario = await _satdb.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        try
        {
            _satdb.Usuarios.Remove(usuario);
            await _satdb.SaveChangesAsync();
            return RedirectToAction("ListarUsuarios");
        }
        catch (DbUpdateException ex)
        {
            return RedirectToAction("ExcluirUsuario", new { id, error = true });
        }
    }
    
    [HttpGet("editarusuario/{id}")]
    public async Task<IActionResult> Editar(int id)
    {
        var usuario = await _satdb.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View("EditarUsuario", usuario);
    }

    [HttpPost("editarusuario/{id}")]
    public async Task<IActionResult> Editar(int id, UsuarioModel usuarioAtualizado)
    {
        if (id != usuarioAtualizado.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("EditarUsuario", usuarioAtualizado);
        }

        try
        {
            _satdb.Update(usuarioAtualizado);
            await _satdb.SaveChangesAsync();
            return RedirectToAction("ListarUsuarios");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Erro ao atualizar.");
            return View("EditarUsuario", usuarioAtualizado);
        }
    }
}