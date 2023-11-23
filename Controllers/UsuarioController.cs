using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private UsuarioRepository _userRepository;


    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        _userRepository = new UsuarioRepository();
    }

    public IActionResult Index()
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        var listUsers = _userRepository.GetAllUsers();
        return View(listUsers);
    }


    [HttpGet]
    public IActionResult CreateUser()
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!IsAdmin()) return RedirectToAction("Index");
        return View(new CreateUserViewModels());
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserViewModels user)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!ModelState.IsValid) return RedirectToAction("Index");
        if (!IsAdmin()) return RedirectToAction("Index");
        _userRepository.Create(new Usuario(user));
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult UpdateUser(int idUsuario)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!IsAdmin()) return RedirectToAction("Index");

        var user = _userRepository.GetUsuarioById(idUsuario);

        if (user != null)
        {
            return View(new UpdateUserViewModels(user.Id));
        }
        else
        {
            return RedirectToAction("Index");
        }

    }


    [HttpPost]
    public IActionResult UpdateUser(UpdateUserViewModels user)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!ModelState.IsValid) return RedirectToAction("Index");
        if (!IsAdmin()) return RedirectToAction("Index");
        _userRepository.Update(user.Id, new Usuario(user));
        return RedirectToAction("Index");
    }


    public IActionResult DeleteUser(int idUsuario)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!IsAdmin()) return RedirectToAction("Index");
        _userRepository.DeleteUsuarioById(idUsuario);
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool IsAdmin()
    {
        return HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "admin";
    }
    private bool IsLogged()
    {
        return HttpContext.Session != null && (HttpContext.Session.GetString("Rol") == "admin" || HttpContext.Session.GetString("Rol") == "operador");
    }
}