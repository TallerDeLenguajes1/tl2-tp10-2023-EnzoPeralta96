using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;


public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private UsuarioRepository _usuarioRepository;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
        _usuarioRepository = new UsuarioRepository();
    }

    private void CreateSession(Usuario userLoged)
    {
        HttpContext.Session.SetString("Usuario", userLoged.Nombre_de_usuario);
        HttpContext.Session.SetString("Password", userLoged.Password.ToString());
        HttpContext.Session.SetString("Rol", userLoged.Rol.ToString());
    }

    public IActionResult Index()
    {
        return View(new LoginViewModels());
    }

    [HttpPost]
    public IActionResult LoginUser(LoginViewModels userViewModels)
    {
        var users = _usuarioRepository.GetAllUsers();
        var user = users.FirstOrDefault(u => u.Nombre_de_usuario == userViewModels.Usuario && u.Password == userViewModels.Password);
        if (user == null) return RedirectToAction("Index");
        CreateSession(user);
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }
}
