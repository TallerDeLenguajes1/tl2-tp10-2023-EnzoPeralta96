using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;
public class LoginController : HelperController
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository):
    base(logger, usuarioRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
    }

    public IActionResult Index()
    {
        return View(new LoginViewModels());
    }

    [HttpPost]
    public IActionResult LoginUser(LoginViewModels loginVM)
    {
        try
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var user = _usuarioRepository.UserExists(loginVM.Usuario, loginVM.Password);

            if (user == null) /*Consultar*/
            {
                _logger.LogWarning("Intento de acceso invalido - Usuario:" + loginVM.Usuario + " Clave ingresada: " + loginVM.Password);
                var loginVMMensaje = new LoginViewModels
                {
                    MensajeError = "Usuario no válido"
                };
                return View("Index",loginVMMensaje);
            }

            _logger.LogInformation("el usuario " + user.Nombre_de_usuario + " ingreso correctamente");

            CreateSession(user);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al intentar logear un usuario {ex.ToString()}");
            return BadRequest("Fallo el inicio de sesion");
        }
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Remove("Id");
        HttpContext.Session.Remove("Rol");
        HttpContext.Session.Remove("Autenticado");
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }

}

