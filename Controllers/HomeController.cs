using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SATWeb.Models;

namespace SATWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
}