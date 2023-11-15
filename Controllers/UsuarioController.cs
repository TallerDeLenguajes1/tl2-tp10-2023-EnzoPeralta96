using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;

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
        var listUsers = _userRepository.GetAllUsuarios();
        return View(listUsers);
    }


    [HttpGet]
    public IActionResult CreateUser()
    {
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult CreateUser(Usuario user)
    {
        _userRepository.Create(user);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult UpdateUser(int idUsuario)
    {
        var user = _userRepository.GetUsuarioById(idUsuario);
        return View(user);
    }


   [HttpPost]
    public IActionResult UpdateUser(Usuario user)
    {
        _userRepository.Update(user.Id, user);
        return RedirectToAction("Index");
    }

    
    public IActionResult DeleteUser(int idUsuario)
    {
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

}
