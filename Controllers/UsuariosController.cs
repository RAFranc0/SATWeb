using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATWeb.Data;
using SATWeb.Models;

namespace SATWeb.Controllers;

public class UsuariosController(SatWebDbContext satdb) : Controller
{
    //============================================
    //REGIÃO: Actions de API/JSON
    //============================================
    
    [HttpGet]
    public async Task<IActionResult> ObterUsuarioPorId(int id)
    {
        var usuario = await satdb.Usuarios
            .Include(u => u.Departamento)
            .FirstOrDefaultAsync(u => u.Id == id);
    
        if (usuario == null)
        {
            return NotFound();
        }

        return Json(new { 
            id = usuario.Id, 
            nome = usuario.Nome, 
            departamentoId = usuario.DepartamentoId,
            nomeDepartamento = usuario.Departamento.Nome 
        });
    }
    
    //===============================================
    //REGIÃO: Actions de Cadastro de Usuários
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> CadastrarUsuario()
    {
        var viewModel = new CadastroUsuarioViewModel()
        {
            Usuario = new UsuarioModel(),
            ListaDepartamentos = await satdb.Departamentos.ToListAsync()
        };
            
        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Cadastrar(CadastroUsuarioViewModel cadastroUsuario)
    {
        try
        {
            var novoUsuario = new UsuarioModel
            {
                Nome = cadastroUsuario.Usuario.Nome,
                DepartamentoId = cadastroUsuario.Usuario.DepartamentoId
            };

            satdb.Usuarios.Add(novoUsuario);
            await satdb.SaveChangesAsync();
            return RedirectToAction("ListarUsuarios");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Erro ao cadastrar: {ex}");
            if(ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }

            ModelState.AddModelError("", "Não foi possível cadastrar o usuário. Verifique os dados.");
            cadastroUsuario.ListaDepartamentos = await satdb.Departamentos.ToListAsync();
            return View("CadastrarUsuario", cadastroUsuario);
        }
    }
    
    //===============================================
    //REGIÃO: Actions de Listagem de Usuários
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> ListarUsuarios()
    {
        var listaDeUsuarios = await satdb.Usuarios
            .Include(d => d.Departamento)
            .ToListAsync();
        return View("ListarUsuarios", listaDeUsuarios);
    }
    
    //===============================================
    //REGIÃO: Actions de Edição de Usuários
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> EditarUsuario(int? id = null)
    {
        var viewModel = new EditarUsuarioViewModel
        {
            ListaDepartamentos = await satdb.Departamentos.ToListAsync(),
            TodosUsuarios = await satdb.Usuarios.ToListAsync()
        };

        if (id.HasValue)
        {
            viewModel.Usuario = await satdb.Usuarios
                .Include(u => u.Departamento)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Editar(EditarUsuarioViewModel viewModel)
    {
        var usuarioExistente = await satdb.Usuarios.FindAsync(viewModel.Usuario.Id);
        if (usuarioExistente == null)
        {
            return NotFound();
        }

        try
        {
            usuarioExistente.Nome = viewModel.Usuario.Nome;
            usuarioExistente.DepartamentoId = viewModel.Usuario.DepartamentoId;
        
            await satdb.SaveChangesAsync();
            return RedirectToAction("ListarUsuarios");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Erro ao atualizar usuário.");
            viewModel.ListaDepartamentos = await satdb.Departamentos.ToListAsync();
            viewModel.TodosUsuarios = await satdb.Usuarios.ToListAsync();
            return View("EditarUsuario", viewModel);
        }
    }
    
    //===============================================
    //REGIÃO: Actions de Exclusão de Usuários
    //===============================================
    
    [HttpGet]
    public async Task<IActionResult> ExcluirUsuario(int? id = null, bool error = false)
    {
        var listaDeUsuarios = await satdb.Usuarios
            .Include(u => u.Departamento)
            .ToListAsync();
        UsuarioModel? usuarioSelecionado = null;

        if (id.HasValue)
        {
            usuarioSelecionado = await satdb.Usuarios.FindAsync(id);
        }

        ViewBag.Usuarios = listaDeUsuarios;
        ViewBag.UsuarioSelecionado = usuarioSelecionado;
        ViewBag.Erro = error;

        return View("ExcluirUsuario");
    }
    
    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        var usuario = await satdb.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        try
        {
            satdb.Usuarios.Remove(usuario);
            await satdb.SaveChangesAsync();
            return RedirectToAction("ListarUsuarios");
        }
        catch (DbUpdateException)
        {
            return RedirectToAction("ExcluirUsuario", new { id, error = true });
        }
    }
}