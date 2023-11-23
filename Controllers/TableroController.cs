using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;
using TableroRepositorio;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

/*En el controlador de tableros: Listar, Crear, Modificar y Eliminar Tableros. (Por el
momento asuma que el usuario propietario es siempre el mismo)
*/
public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private TableroRepository _tableroRepository;
    private UsuarioRepository _usuarioRepository;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        _tableroRepository = new TableroRepository();
        _usuarioRepository = new UsuarioRepository();
    }

    private bool IsAdmin()
    {
        return HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "admin";
    }
    private bool IsLogged()
    {
        return HttpContext.Session != null && (HttpContext.Session.GetString("Rol") == "admin" || HttpContext.Session.GetString("Rol") == "operador");
    }

    public IActionResult Index()
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

        if (IsAdmin())
        {
            var tableros = _tableroRepository.GetTableros();
            return View(tableros);
        }
        else
        {
            var usuarios = _usuarioRepository.GetAllUsers();
            var usuario = usuarios.FirstOrDefault(u => u.Nombre_de_usuario == HttpContext.Session.GetString("Usuario") && u.Password == HttpContext.Session.GetString("Password"));
            var tablero = _tableroRepository.GetTableroByUser(usuario.Id);
            return View(tablero);
        }

    }

    [HttpGet]
    public IActionResult CreateTablero()
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!IsAdmin()) return RedirectToAction("Index");
        var usuarios = _usuarioRepository.GetAllUsers();
        return View(new CreateTableroViewModels(usuarios));
    }

    [HttpPost]
    public IActionResult CreateTablero(CreateTableroViewModels tablero)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!ModelState.IsValid) return RedirectToAction("Index");
        if (!IsAdmin()) return RedirectToAction("Index");
        _tableroRepository.Create(new Tablero(tablero));
        return RedirectToAction("Index");
    }



    [HttpGet]
    public IActionResult UpdateTablero(int idTablero)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!IsAdmin()) return RedirectToAction("Index");
        var usuarios = _usuarioRepository.GetAllUsers();
        return View(new UpdateTableroViewModels(idTablero,usuarios));
    }


    [HttpPost]
    public IActionResult UpdateTablero(UpdateTableroViewModels tablero)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!ModelState.IsValid) return RedirectToAction("Index");
        if (!IsAdmin()) return RedirectToAction("Index");
        _tableroRepository.Update(tablero.Id, new Tablero(tablero));
        return RedirectToAction("Index");
    }
    
    public IActionResult DeleteTablero(int idTablero)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!IsAdmin()) return RedirectToAction("Index");
        _tableroRepository.Delete(idTablero);
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
