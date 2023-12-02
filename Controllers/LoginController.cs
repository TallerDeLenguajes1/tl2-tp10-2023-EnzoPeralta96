using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;


public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
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
        try
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var user = _usuarioRepository.GetAllUsers().FirstOrDefault(u => u.Nombre_de_usuario == userViewModels.Usuario && u.Password == userViewModels.Password);

            if (user == null)
            {
                _logger.LogWarning("Intento de acceso invalido - Usuario:" + userViewModels.Usuario + " Clave ingresada: " + userViewModels.Password);
                return RedirectToAction("Index");
            }

            _logger.LogInformation("el usuario " + user.Nombre_de_usuario + " ingreso correctamente");

            CreateSession(user);

            return RedirectToRoute(new { controller = "Tablero", action = "Index" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest("Fallo el inicio de sesion");
        }
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Remove("Usuario");
        HttpContext.Session.Remove("Password");
        HttpContext.Session.Remove("Rol");
        return Redirect("Index");
    }
}

